namespace CustomerProject.Entity
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string TC { get; set; }
        public string BirthPlace { get; set; }
        public int BirthYear { get; set; }
        public bool IsNameExtraordinary { get; set; }

        public List<ContactInformation> ContactInformations { get; set; }
        public List<AddressInformation> AddressInformations { get; set; }

    }
}
