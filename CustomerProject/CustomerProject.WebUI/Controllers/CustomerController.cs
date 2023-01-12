using CustomerProject.Business.Abstract;
using CustomerProject.Entity;
using CustomerProject.WebUI.Extensions;
using CustomerProject.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Numerics;

namespace CustomerProject.WebUI.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerService customerService;

		public CustomerController(ICustomerService customerService, IContactService contactService)
        {
            this.customerService = customerService;
        }

		//Arama varsa arama verilerini yoksa bütün müşterileri CustomerModel ViewModel'e çevirerek View'e gönderir.
		public async Task<IActionResult> CustomerList(string? search)
        {
            var customers = await customerService.GetAllAsync(search);

            List<CustomerModel> list = new List<CustomerModel>();

            foreach (var customer in customers)
            {
                list.Add(new CustomerModel()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Surname = customer.Surname,
                    TC = long.Parse(customer.TC),
                    BirthPlace = customer.BirthPlace,
                    BirthYear = customer.BirthYear,
                    IsNameExtraordinary = customer.IsNameExtraordinary,
                });
            }

            return View(list);
        }

		//CustomerList'de bulunan müşteri ekleme butonu tıklandığnda gelir ve Müşteri ekleme formunun bulunuduğu View'e gönderir.
		[HttpGet]
		public IActionResult CustomerAdd()
		{
			return View();
		}

		//CustomerAdd View form'u post edildiğinde gelir ve aldığı modeli gerekli validation kontrolleri yapıldıktan sonra duruma göre veritabanına ekler.
		[HttpPost]
		public async Task<IActionResult> CustomerAdd(CustomerModel model)
		{
            if(ModelState.IsValid)
            {
                if(!await customerService.IsHaveTC(model.TC.ToString()))
                {
                    var newCustomer = new Customer()
                    {
                        Name = model.Name,
                        Surname = model.Surname,
                        TC = model.TC.ToString(),
                        BirthPlace = model.BirthPlace,
                        BirthYear = model.BirthYear,
                    };
                    await customerService.CreateAsync(newCustomer);

                    TempData.Put<AlertMessage>("message", new AlertMessage()
                    {
                        Title = "Kayıt Durumu",
                        Message = "Kayıt başarılı bir şekilde eklendi.",
                        AlertType = "success"
                    });

                    return RedirectToAction("CustomerList");
                }

                ModelState.AddModelError("", "Girdiğiniz T.C kimlik numarası kayıtlıdır.");
            }

			return View(model);
		}

		//CustomerList'de bulunan müşteri güncellem butonu tıklandığnda müşteri id'si ile gelir ve müşteriyi veritabanından alarak View'e gönderir.
		[HttpGet]
        public async Task<IActionResult> CustomerEdit(int Id)
        {
            var customer = await customerService.GetByIdAsync(Id);

            if(customer != null)
            {
                return View(new CustomerModel
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Surname = customer.Surname,
                    TC = long.Parse(customer.TC),
                    BirthPlace = customer.BirthPlace,
                    BirthYear = customer.BirthYear,
                });
            }

            TempData.Put<AlertMessage>("message", new AlertMessage()
            {
                Title = "Kayıt Durumu",
                Message = "Güncellenmek istenen müşteriye ulaşılamadı.",
                AlertType = "warning"
            });

            return RedirectToAction("CustomerList");
        }

        //CustomerEdit View form'u post edildiğinde bu fonksiyon çalışır. Gerekli bütün validation kısıtlamaları kontrol edildikten sonra duruma göre veriyi veritabanında günceller.
        [HttpPost]
        public async Task<IActionResult> CustomerEdit(CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = await customerService.GetByIdAsync(model.Id??-1);

                if(customer != null)
                {
                    //TC üzerinde değişiklik yapılmış mı? Eğer yapıldıysa yeni TC'nin veritabanında kaytlı olup olmadığını kontrol et yoksa değişiklikleri yapan metodu çağır ve müşteriyi güncelle.Aksi halde hata mesajı ver.
                    if(customer.TC != model.TC.ToString())
                    {
                        if (!await customerService.IsHaveTC(model.TC.ToString()))
                        {
                            await UpdateCustomerDatas(customer, model);

							TempData.Put<AlertMessage>("message", new AlertMessage()
							{
								Title = "Kayıt Durumu",
								Message = "Müşteri bilgisi başarılı bir şekilde güncellendi.",
								AlertType = "success"
							});

							return RedirectToAction("CustomerList");
                        }

                        ModelState.AddModelError("", "Girdiğiniz T.C kimlik numarası kayıtlıdır.");
                    }
                    else
                    {
                        await UpdateCustomerDatas(customer, model);
						TempData.Put<AlertMessage>("message", new AlertMessage()
						{
							Title = "Kayıt Durumu",
							Message = "Müşteri bilgisi başarılı bir şekilde güncellendi.",
							AlertType = "success"
						});
						return RedirectToAction("CustomerList");
                    }  
                }
                else
                {
                    TempData.Put<AlertMessage>("message", new AlertMessage()
                    {
                        Title = "Kayıt Durumu",
                        Message = "Kayıtlı müşteriye ulaşılamadığından güncellenemedi.",
                        AlertType = "warning"
                    });

                    return RedirectToAction("CustomerList");
                }
            }

            return View(model);
        }

        //Veritabanındaki veriyi, form'dan gelenlerle günceller.
        public async Task UpdateCustomerDatas(Customer customer, CustomerModel model)
        {
            customer.Name = model.Name;
            customer.Surname = model.Surname;
            customer.TC = model.TC.ToString();
            customer.BirthPlace = model.BirthPlace;
            customer.BirthYear = model.BirthYear;

            await customerService.UpdateAsync(customer);

            TempData.Put<AlertMessage>("message", new AlertMessage()
            {
                Title = "Kayıt Durumu",
                Message = "Kayıt başarılı bir şekilde güncellendi.",
                AlertType = "success"
            });

        }

		//CustomerList'de bulunan müşteri sil butonu tıklandığnda müşteri id'si ile gelir ve müşteriyi veritabanından siler.
		[HttpPost]
        public async Task<IActionResult> CustomerDelete(int Id)
        {
            var customer = await customerService.GetByIdAsync(Id);

            if(customer != null)
            {
                await customerService.DeleteAsync(customer);

                TempData.Put<AlertMessage>("message", new AlertMessage()
                {
                    Title = "Kayıt Durumu",
                    Message = "Kayıt başarılı bir şekilde silindi.",
                    AlertType = "success"
                });

                return RedirectToAction("CustomerList");
            }

            TempData.Put<AlertMessage>("message", new AlertMessage()
            {
                Title = "Kayıt Durumu",
                Message = "kayıtlı müşteri bulunamadığından silinemedi.",
                AlertType = "warning"
            });

            return RedirectToAction("CustomerList");
        }

	}
}