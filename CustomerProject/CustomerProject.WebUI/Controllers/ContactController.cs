using CustomerProject.Business.Abstract;
using CustomerProject.Entity;
using CustomerProject.WebUI.Extensions;
using CustomerProject.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerProject.WebUI.Controllers
{
	public class ContactController:Controller
	{
		private ICustomerService customerService;
		private IContactService contactService;

		public ContactController(ICustomerService customerService, IContactService contactService)
		{
			this.customerService = customerService;
			this.contactService = contactService;
		}

		//Customer Controller'da bulunan CustomerList ile sergilenen müşterilerde, iletişim bilgileri butonu tıklandığında müşteri id'si ile birlikte bu action'a yönlenir.
		//Müşteri iletişim bilgileri ile birlikte getirilir ve View'e gönderilir.
		public async Task<IActionResult> CustomerContactList(int Id)
		{
			var customer = await customerService.GetCustomerByIdWithContact(Id);

			if (customer != null)
			{
				return View(customer);
			}

			TempData.Put<AlertMessage>("message", new AlertMessage()
			{
				Title = "İletişim Bilgileri",
				Message = "Kayıtlı müşterinin iletişim bilgilerine ulaşılamadı.",
				AlertType = "warning"
			});

			return RedirectToAction("CustomerList","Customer");
		}

		//CustomerContactList ile döndürülen View'de İletişim bilgisi ekle butonu tıklanırsa bu action çalışır.Telefonn kodları ile birlikte model View'e gönderilir.
		//View kısmında, gelen model mail için ise sadece mail inputu aksi halde telefon tipi, kodu ve numarası form üzerinden alınır.
		[HttpGet]
		public async Task<IActionResult> CustomerContactAdd(int CustomerId, bool IsMail)
		{
			var phonoCodes = await contactService.GetPhoneCodes() ?? new List<PhoneCode>();

			return View(new ContactInfoModel()
			{
				PhoneCodes = phonoCodes,
				CustomerId = CustomerId,
				IsMail = IsMail
			});
		}

		//CustomerContactAdd formu post edildiğide çalışır ve oradan gelen ContactInfoModel için eklenen bilginin mail mi yoksa telefon mu olduğunu ve gerekli diğer validation gerekliklerini kontrol eder, uygunsa ekler.
		[HttpPost]
		public async Task<IActionResult> CustomerContactAdd(ContactInfoModel model)
		{
			var phonoCodes = await contactService.GetPhoneCodes() ?? new List<PhoneCode>();
			if (ModelState.IsValid)
			{
				var customer = await customerService.GetCustomerByIdWithContact(model.CustomerId);

				if (customer != null)
				{
					if (model.IsMail)
					{
						if (!string.IsNullOrEmpty(model.Mail) && !await contactService.IsHaveEmail(model.Mail))
						{
							await contactService.CreateAsync(new ContactInformation()
							{
								Customer = customer,
								CustomerId = customer.Id,
								IsMail = model.IsMail,
								Mail = model.Mail,
							});

							TempData.Put<AlertMessage>("message", new AlertMessage()
							{
								Title = "Kayıt Durumu",
								Message = "Mail bilgisi başarılı bir şekilde eklendi.",
								AlertType = "success"
							});

							return Redirect("/Contact/CustomerContactList?Id=" + customer.Id);
						}
						else
						{
							ModelState.AddModelError("", "Mail boş bırakılamaz veya mail kayıtlı");

							return View(model);
						}
					}
					else
					{
						if (!string.IsNullOrEmpty(model.Phone.ToString()) && !await contactService.IsHavePhone(model.PhoneCode+" "+model.Phone.ToString()) && !string.IsNullOrEmpty(model.PhoneType))
						{
							await contactService.CreateAsync(new ContactInformation()
							{
								Customer = customer,
								CustomerId = customer.Id,
								IsMail = model.IsMail,
								Phone = model.PhoneCode+" "+model.Phone.ToString(),
								PhoneType = model.PhoneType,
							});

							TempData.Put<AlertMessage>("message", new AlertMessage()
							{
								Title = "Kayıt Durumu",
								Message = "Telefon bilgisi başarılı bir şekilde eklendi.",
								AlertType = "success"
							});

							return Redirect("/Contact/CustomerContactList?Id=" + customer.Id);
						}
						else
						{
							ModelState.AddModelError("", "Telefon ve Telefon Başlığı boş bırakılamaz veya telefon kayıtlı");

							model.PhoneCodes = phonoCodes;
							return View(model);
						}
					}
				}

				TempData.Put<AlertMessage>("message", new AlertMessage()
				{
					Title = "Kayıt Durumu",
					Message = "Kayıtlı müşteri bulunamadığından bilgiler eklenemedi.",
					AlertType = "warning"
				});

				return RedirectToAction("CustomerList","Customer");
			}

			model.PhoneCodes = phonoCodes;
			return View(model);
		}

		//CustomerContactList ile döndürülen View'de İletişim bilgisi güncelle butonu tıklanırsa bu action çalışır.Bilgi mail ise sadece mail bilgileri, telefon ise telefon kodları ve bilgileri View'e gönderilir.
		[HttpGet]
		public async Task<IActionResult> CustomerContactEdit(int Id)
		{
			var contactInfo = await contactService.GetByIdAsync(Id);

			if (contactInfo != null)
			{
				if (contactInfo.IsMail)
				{
					return View(new ContactInfoModel()
					{
						CustomerId = contactInfo.CustomerId,
						ContactId = Id,
						IsMail = contactInfo.IsMail,
						Mail = contactInfo.Mail,
					});
				}
				else
				{
					var phonoCodes = await contactService.GetPhoneCodes() ?? new List<PhoneCode>();
					var splitPhone = contactInfo.Phone.Split(' ');

					return View(new ContactInfoModel()
					{
						PhoneCodes = phonoCodes,
						CustomerId = contactInfo.CustomerId,
						ContactId = Id,
						IsMail = contactInfo.IsMail,
						PhoneType = contactInfo.PhoneType,
						PhoneCode = splitPhone[0],
						Phone = long.Parse(splitPhone[1])
					});
				}

			}

			TempData.Put<AlertMessage>("message", new AlertMessage()
			{
				Title = "Kayıt Durumu",
				Message = "İletişim Bilgisine Erişilemesi.",
				AlertType = "warning"
			});

			return RedirectToAction("CustomerList", "Customer");
		}

		//CustomerContactEdit post edildiğinde çalışır ve gelen model üzerinde validation gerekliklerini kontrol eder, başarılı olur ise günceller, aksi halde modeli tekrar hazırlar ve View'e döner.
		[HttpPost]
		public async Task<IActionResult> CustomerContactEdit(ContactInfoModel model)
		{
			var phonoCodes = await contactService.GetPhoneCodes() ?? new List<PhoneCode>();
			if (ModelState.IsValid)
			{
				var customer = await customerService.GetCustomerByIdWithContact(model.CustomerId);
				var contact = await contactService.GetByIdAsync(model.ContactId ?? -1);

				if (customer != null && contact != null)
				{
					if (model.IsMail)
					{
						if (!string.IsNullOrEmpty(model.Mail) && (contact.Mail == model.Mail || !await contactService.IsHaveEmail(model.Mail)))
						{
							contact.Mail = model.Mail;
							await contactService.UpdateAsync(contact);

							TempData.Put<AlertMessage>("message", new AlertMessage()
							{
								Title = "Kayıt Durumu",
								Message = "Mail bilgisi başarılı bir şekilde güncellendi.",
								AlertType = "success"
							});

							return Redirect("/Contact/CustomerContactList?Id=" + customer.Id);
						}
						else
						{
							ModelState.AddModelError("", "Mail boş bırakılamaz veya mail kayıtlı");

							return View(model);
						}
					}
					else
					{
						if (!string.IsNullOrEmpty(model.Phone.ToString()) && (contact.Phone == model.Phone.ToString() || !await contactService.IsHavePhone(model.PhoneCode + " " + model.Phone.ToString()) && !string.IsNullOrEmpty(model.PhoneType)))
						{
							contact.PhoneType = model.PhoneType;
							contact.Phone = model.PhoneCode + " " + model.Phone.ToString();
							await contactService.UpdateAsync(contact);

							TempData.Put<AlertMessage>("message", new AlertMessage()
							{
								Title = "Kayıt Durumu",
								Message = "Telefon bilgisi başarılı bir şekilde güncellendi.",
								AlertType = "success"
							});

							return Redirect("/Contact/CustomerContactList?Id=" + customer.Id);
						}
						else
						{
							ModelState.AddModelError("", "Telefon ve Telefon Başlığı boş bırakılamaz veya telefon kayıtlı");

							model.PhoneCodes = phonoCodes;
							return View(model);
						}
					}
				}

				TempData.Put<AlertMessage>("message", new AlertMessage()
				{
					Title = "Kayıt Durumu",
					Message = "Kayıtlı müşteri veya iletişim bilgisi bulunamadığından bilgiler güncellenemedi.",
					AlertType = "warning"
				});

				return RedirectToAction("CustomerList", "Customer");
			}

			model.PhoneCodes = phonoCodes;
			return View(model);
		}

		//İletişim bilgisi id'si ile bilgiyi sorgular, var ise siler, yoksa hata bilgisi döner.
		[HttpPost]
		public async Task<IActionResult> CustomerContactDelete(int Id)
		{
			var contact = await contactService.GetByIdAsync(Id);

			if (contact != null)
			{
				await contactService.DeleteAsync(contact);

				TempData.Put<AlertMessage>("message", new AlertMessage()
				{
					Title = "Kayıt Durumu",
					Message = "Kayıt başarılı bir şekilde silindi.",
					AlertType = "success"
				});

				return Redirect("/Contact/CustomerContactList?Id=" + contact.CustomerId);
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
