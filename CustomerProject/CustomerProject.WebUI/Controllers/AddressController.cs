using CustomerProject.Business.Abstract;
using CustomerProject.Entity;
using CustomerProject.WebUI.Extensions;
using CustomerProject.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerProject.WebUI.Controllers
{
	//Bütün medoları ile ContactController'a benzer yapıda çalışır.
	public class AddressController:Controller
	{
		private ICustomerService customerService;
		private IAddressService addressService;

		public AddressController(ICustomerService customerService, IAddressService addressService)
		{
			this.customerService = customerService;
			this.addressService = addressService;
		}

		public async Task<IActionResult> CustomerAddressList(int Id)
		{
			var customer = await customerService.GetCustomerByIdWithAddress(Id);

			if (customer != null)
			{
				return View(customer);
			}

			TempData.Put<AlertMessage>("message", new AlertMessage()
			{
				Title = "İletişim Bilgileri",
				Message = "Kayıtlı müşterinin adres bilgilerine ulaşılamadı.",
				AlertType = "warning"
			});

			return RedirectToAction("CustomerList","Customer");
		}

		[HttpGet]
		public IActionResult CustomerAddressAdd(int CustomerId)
		{
			return View(new AddressInfoModel()
			{
				CustomerId = CustomerId,
			});
		}

		[HttpPost]
		public async Task<IActionResult> CustomerAddressAdd(AddressInfoModel model)
		{
			if (ModelState.IsValid)
			{
				var customer = await customerService.GetCustomerByIdWithContact(model.CustomerId);

				if (customer != null)
				{
					await addressService.CreateAsync(new AddressInformation()
					{
						Customer = customer,
						CustomerId = model.CustomerId,
						AddressType = model.AddressType,
						Country = model.Country,
						City = model.City,
						District = model.District,
						DetailedAddress = model.DetailedAddress,
						PostCode = model.PostCode,
					});

					TempData.Put<AlertMessage>("message", new AlertMessage()
					{
						Title = "Kayıt Durumu",
						Message = "Adres bilgisi başarılı bir şekilde eklendi.",
						AlertType = "success"
					});

					return Redirect("/Address/CustomerAddressList?Id=" + customer.Id);
				}

				TempData.Put<AlertMessage>("message", new AlertMessage()
				{
					Title = "Kayıt Durumu",
					Message = "Kayıtlı müşteri bulunamadığından bilgiler eklenemedi.",
					AlertType = "warning"
				});

				return RedirectToAction("CustomerList","Customer");
			}

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> CustomerAddressEdit(int Id)
		{
			var addressInfo = await addressService.GetByIdAsync(Id);

			if (addressInfo != null)
			{
				return View(new AddressInfoModel()
				{
					CustomerId = addressInfo.CustomerId,
					AddressId = addressInfo.Id,
					AddressType = addressInfo.AddressType,
					Country = addressInfo.Country,
					City = addressInfo.City,
					District = addressInfo.District,
					DetailedAddress = addressInfo.DetailedAddress,
					PostCode = addressInfo.PostCode,
				});
			}

			TempData.Put<AlertMessage>("message", new AlertMessage()
			{
				Title = "Kayıt Durumu",
				Message = "Adres Bilgisine Erişilemesi.",
				AlertType = "warning"
			});

			return RedirectToAction("CustomerList", "Customer");
		}

		[HttpPost]
		public async Task<IActionResult> CustomerAddressEdit(AddressInfoModel model)
		{
			if (ModelState.IsValid)
			{
				var customer = await customerService.GetCustomerByIdWithContact(model.CustomerId);
				var address = await addressService.GetByIdAsync(model.AddressId ?? -1);

				if (customer != null && address != null)
				{
					address.AddressType = model.AddressType;
					address.Country = model.Country;	
					address.City = model.City;
					address.District = model.District;
					address.DetailedAddress = model.DetailedAddress;
					address.PostCode = model.PostCode;

					await addressService.UpdateAsync(address);

					TempData.Put<AlertMessage>("message", new AlertMessage()
					{
						Title = "Kayıt Durumu",
						Message = "Adres bilgisi başarılı bir şekilde güncellendi.",
						AlertType = "success"
					});

					return Redirect("/Address/CustomerAddressList?Id=" + customer.Id);
				}

				TempData.Put<AlertMessage>("message", new AlertMessage()
				{
					Title = "Kayıt Durumu",
					Message = "Kayıtlı müşteri veya adres bilgisi bulunamadığından bilgiler güncellenemedi.",
					AlertType = "warning"
				});

				return RedirectToAction("CustomerList", "Customer");
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> CustomerAddressDelete(int Id)
		{
			var address = await addressService.GetByIdAsync(Id);

			if (address != null)
			{
				await addressService.DeleteAsync(address);

				TempData.Put<AlertMessage>("message", new AlertMessage()
				{
					Title = "Kayıt Durumu",
					Message = "Kayıt başarılı bir şekilde silindi.",
					AlertType = "success"
				});

				return Redirect("/Address/CustomerAddressList?Id=" + address.CustomerId);
			}

			TempData.Put<AlertMessage>("message", new AlertMessage()
			{
				Title = "Kayıt Durumu",
				Message = "kayıtlı müşteri bulunamadığından silinemedi.",
				AlertType = "warning"
			});

			return RedirectToAction("CustomerList", "Customer");
		}

	}
}
