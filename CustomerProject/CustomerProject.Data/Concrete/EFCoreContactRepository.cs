using Azure;
using Microsoft.EntityFrameworkCore;
using CustomerProject.Data.Abstract;
using CustomerProject.Entity;

namespace CustomerProject.Data.Concrete
{
    public class EFCoreContactRepository : EFCoreGenericRepository<ContactInformation>, IContactRepository
    {
        public EFCoreContactRepository(CustomerProjectDbContext context) : base(context)
        {

        }

        private CustomerProjectDbContext Context { get { return context as CustomerProjectDbContext; } }

		//Kendisine gelen E-Posta'nın kaytlı olup olmadığını sorgular ve kayıtlı ise true döner.
		public async Task<bool> IsHaveEmail(string email)
		{
            return await Context.ContactInformations.AnyAsync(c => c.Mail.Equals(email));
		}

		//Kendisine gelen telefonun kaytlı olup olmadığını sorgular ve kayıtlı ise true döner.
		public async Task<bool> IsHavePhone(string phone)
		{
			return await Context.ContactInformations.AnyAsync(c => c.Phone.Equals(phone));
		}

		//Veritabanından uluslararası telefon kodlarını liste olarak döndürür.Sadece iletişim bilgilerinde kullanıldığı için bu Repo'da yazdım.
		public async Task<List<PhoneCode>> GetPhoneCodes()
		{
			return await Context.PhoneCodes.ToListAsync();
		}

	}
}
