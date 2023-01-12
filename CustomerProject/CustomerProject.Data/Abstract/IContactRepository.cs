using CustomerProject.Entity;

namespace CustomerProject.Data.Abstract
{
    public interface IContactRepository : IRepository<ContactInformation>
    {
		Task<bool> IsHaveEmail(string email);

		Task<bool> IsHavePhone(string phone);

		Task<List<PhoneCode>> GetPhoneCodes();
	}
}
