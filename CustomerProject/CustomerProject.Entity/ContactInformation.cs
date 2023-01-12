namespace CustomerProject.Entity
{
    public class ContactInformation
    {
        public int Id { get; set; }
        public bool IsMail { get; set; }
        public string? Mail { get; set; }
        public string? PhoneType { get; set; }
        public string? Phone { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }
}
