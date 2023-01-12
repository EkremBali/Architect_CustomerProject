using System.ComponentModel.DataAnnotations;

namespace CustomerProject.WebUI.Models
{
	//ContactInfomation entity'sinin model karşılığı.Validation için gerekli DataAnnotation'lar kullanılır.
	public class CustomerModel
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }

		[Range(10000000000,99999999999, ErrorMessage = "T.C. kimlik numarası 11 haneli olmalıdır!!!")]
		public long TC { get; set; }
		public string BirthPlace { get; set; }

		[Range(1000, 9999, ErrorMessage = "Doğum yılı 4 haneli olmalıdır!!!")]
		public int BirthYear { get; set; }
		public bool IsNameExtraordinary { get; set; }
	}
}
