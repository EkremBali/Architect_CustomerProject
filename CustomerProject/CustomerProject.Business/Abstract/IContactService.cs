using CustomerProject.Entity;

namespace CustomerProject.Business.Abstract
{
    public interface IContactService
    {
        Task<ContactInformation> GetByIdAsync(int id);

        Task<List<ContactInformation>> GetAllAsync();

        Task CreateAsync(ContactInformation entity);
        Task UpdateAsync(ContactInformation entity);
        Task DeleteAsync(ContactInformation entity);

		Task<bool> IsHaveEmail(string email);

		Task<bool> IsHavePhone(string phone);

        Task<List<PhoneCode>> GetPhoneCodes();
	}
}
