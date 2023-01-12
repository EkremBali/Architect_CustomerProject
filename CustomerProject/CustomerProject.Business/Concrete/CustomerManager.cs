using CustomerProject.Business.Abstract;
using CustomerProject.Data.Abstract;
using CustomerProject.Entity;

namespace CustomerProject.Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private ICustomerRepository customerRepository;

        public CustomerManager(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

		//Kendisine arama sözcüğü geldi ise GetSearchResult fonksiyonu ile gerekli müşterileri aksi halde GetAllAsync ile tüm müşterileri dönen metod.
		public async Task<List<Customer>> GetAllAsync(string? search)
        {
            if (string.IsNullOrEmpty(search))
            {
				return await customerRepository.GetAllAsync();
			}

            return await customerRepository.GetSearchResult(search);

		}

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await customerRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Customer entity)
        {
            entity.IsNameExtraordinary = CheckNameExtraordinary(entity.Name);
            
            await customerRepository.CreateAsync(entity);
        }

		public async Task DeleteAsync(Customer entity)
        {
            await customerRepository.DeleteAsync(entity);
        }

        public async Task UpdateAsync(Customer entity)
        {
            entity.IsNameExtraordinary = CheckNameExtraordinary(entity.Name);
            await customerRepository.UpdateAsync(entity);
        }


		//Bir isimde aynı sesli harften en az 3 adet varsa true döner.Bu da o ismin sıra dışı olduğu anlamına gelir.Aksi halde false döner. CreateAsync ve UpdateAsync işlemlerinde isim için kullanılır.
		public bool CheckNameExtraordinary(string customerName)
        {
            var vowels = new char[] { 'a', 'e', 'ı', 'i', 'o', 'ö', 'u', 'ü' };

            var vowelCountInName = 0;

            foreach (var vowel in vowels)
            {
                var sameLetterList = customerName.Where(n => n.ToString().ToLower() == vowel.ToString().ToLower()).ToList();
                if (sameLetterList.Count >= 3)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsHaveTC(string tc)
        {
            return await customerRepository.IsHaveTC(tc);
        }

        public async Task<Customer> GetCustomerByIdWithContact(int Id)
        {
            return await customerRepository.GetCustomerByIdWithContact(Id);
        }

		public async Task<Customer> GetCustomerByIdWithAddress(int Id)
		{
            return await customerRepository.GetCustomerByIdWithAddress(Id);
		}
	}
}
