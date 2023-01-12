using CustomerProject.Entity;
using System.ComponentModel.DataAnnotations;

namespace CustomerProject.WebUI.Models
{
	//ContactInfomation entity'sinin model karşılığı. View üzerinde yapılan işlemlerde kullanılan ekstra PhoneCodes listesi ve PhoneCode değişkeni kullanılır.Validation için gerekli DataAnnotation'lar kullanılır.
	public class ContactInfoModel
	{
		public List<PhoneCode>? PhoneCodes;
		public int CustomerId { get; set; }
		public int? ContactId { get; set; }
		public bool IsMail { get; set; }
		
		[DataType(DataType.EmailAddress)]
		public string? Mail { get; set; }

		public string? PhoneType { get; set; }
		public string? PhoneCode { get; set; }

		[Range(1000000000,9999999999, ErrorMessage = "Numara 10 haneli olmalı")]
		public long? Phone { get; set; }
	}
}
