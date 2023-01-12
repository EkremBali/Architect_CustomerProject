namespace CustomerProject.WebUI.Models
{
	//AddressInfomation entity'sinin model karşılığı.
	public class AddressInfoModel
	{
		public int CustomerId { get; set; }
		public int? AddressId { get; set; }
		public string AddressType { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public string District { get; set; }
		public int PostCode { get; set; }
		public string DetailedAddress { get; set; }
	}
}
