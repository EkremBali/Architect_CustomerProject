using CustomerProject.Business.Abstract;
using CustomerProject.Data.Abstract;
using CustomerProject.Data.Concrete;
using CustomerProject.Entity;

namespace CustomerProject.Business.Concrete
{
	//Bu business servisinde hiçbir iş kuralı uygulanmamıştır.Yani olduğu gibi Data katmanındaki contactRepository fonksiyonlarını çalıştırır.
	public class ContactManager : IContactService
    {
        private IContactRepository contactRepository;

        public ContactManager(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public async Task<List<ContactInformation>> GetAllAsync()
        {
            return await contactRepository.GetAllAsync();
        }

        public async Task<ContactInformation> GetByIdAsync(int id)
        {
            return await contactRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(ContactInformation entity)
        {
            await contactRepository.CreateAsync(entity);
        }

		public async Task DeleteAsync(ContactInformation entity)
        {
            await contactRepository.DeleteAsync(entity);
        }

        public async Task UpdateAsync(ContactInformation entity)
        {
            await contactRepository.UpdateAsync(entity);
        }

		public async Task<bool> IsHaveEmail(string email)
		{
            return await contactRepository.IsHaveEmail(email);
		}

		public async Task<bool> IsHavePhone(string phone)
		{
			return await contactRepository.IsHavePhone(phone);
		}

        public async Task<List<PhoneCode>> GetPhoneCodes()
        {
            return await contactRepository.GetPhoneCodes();

		}
	}
}
