using CustomerProject.Entity;

namespace CustomerProject.Business.Abstract
{
    public interface IAddressService
    {
        Task<AddressInformation> GetByIdAsync(int id);

        Task<List<AddressInformation>> GetAllAsync();

        Task CreateAsync(AddressInformation entity);
        Task UpdateAsync(AddressInformation entity);
        Task DeleteAsync(AddressInformation entity);
	}
}
