using Azure;
using Microsoft.EntityFrameworkCore;
using CustomerProject.Data.Abstract;
using CustomerProject.Entity;

namespace CustomerProject.Data.Concrete
{
    public class EFCoreCustomerRepository : EFCoreGenericRepository<Customer>, ICustomerRepository
    {
        public EFCoreCustomerRepository(CustomerProjectDbContext context) : base(context)
        {

        }

        private CustomerProjectDbContext Context { get { return context as CustomerProjectDbContext; } }

        //Kendisine gelen TC'nin kaytlı olup olmadığını sorgular ve kayıtlı ise true döner.
        public async Task<bool> IsHaveTC(string tc)
        {
            return await Context.Customers.AnyAsync(c => c.TC == tc);
        }

		//Id'si verilen müşteriyi iletişim bilgileri ile beraber döndürür.
		public async Task<Customer> GetCustomerByIdWithContact(int Id)
        {
            return await Context.Customers.Include(c => c.ContactInformations).FirstOrDefaultAsync(c => c.Id == Id);
        }

		//Id'si verilen müşteriyi adres bilgileri ile beraber döndürür.s
		public async Task<Customer> GetCustomerByIdWithAddress(int Id)
		{
			return await Context.Customers.Include(c => c.AddressInformations).FirstOrDefaultAsync(c => c.Id == Id);
		}

		//Kendisine gelen arama bilgisini müşteri ismi, soyismi ve TC si üzerinde arar, eşleşen müşterileri liste olarak döndürür.
		public async Task<List<Customer>> GetSearchResult(string search)
		{
			return await Context.Customers.Where(c => c.Name.Contains(search) || c.Surname.Contains(search) || c.TC.Contains(search)).ToListAsync();
		}

	}
}
