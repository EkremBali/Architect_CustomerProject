using CustomerProject.Business.Abstract;
using CustomerProject.Data.Abstract;
using CustomerProject.Entity;

namespace CustomerProject.Business.Concrete
{
	//Bu business servisinde hiçbir iş kuralı uygulanmamıştır.Yani olduğu gibi Data katmanındaki addressRepository fonksiyonlarını çalıştırır.
	public class AddressManager : IAddressService
	{
        private IAddressRepository addressRepository;

        public AddressManager(IAddressRepository addressRepository)
        {
            this.addressRepository = addressRepository;
        }

        public async Task<List<AddressInformation>> GetAllAsync()
        {
            return await addressRepository.GetAllAsync();
        }

        public async Task<AddressInformation> GetByIdAsync(int id)
        {
            return await addressRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(AddressInformation entity)
        {
            await addressRepository.CreateAsync(entity);
        }

		public async Task DeleteAsync(AddressInformation entity)
        {
            await addressRepository.DeleteAsync(entity);
        }

        public async Task UpdateAsync(AddressInformation entity)
        {
            await addressRepository.UpdateAsync(entity);
        }
	}
}
