namespace CustomerProject.Entity
{
    public class AddressInformation
    {
        public int Id { get; set; }
        public string AddressType { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public int PostCode { get; set; }
        public string DetailedAddress { get; set; }

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }
}
