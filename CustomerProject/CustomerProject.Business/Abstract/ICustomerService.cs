using CustomerProject.Entity;

namespace CustomerProject.Business.Abstract
{
    public interface ICustomerService
    {
        Task<Customer> GetByIdAsync(int id);

        Task<List<Customer>> GetAllAsync(string? search);

        Task CreateAsync(Customer entity);
        Task UpdateAsync(Customer entity);
        Task DeleteAsync(Customer entity);

        Task<bool> IsHaveTC(string tc);

        Task<Customer> GetCustomerByIdWithContact(int Id);
		Task<Customer> GetCustomerByIdWithAddress(int Id);
	}
}
