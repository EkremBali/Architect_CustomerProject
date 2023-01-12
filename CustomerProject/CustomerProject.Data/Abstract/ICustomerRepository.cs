using CustomerProject.Entity;

namespace CustomerProject.Data.Abstract
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<bool> IsHaveTC(string tc);

        Task<Customer> GetCustomerByIdWithContact(int Id);

		Task<Customer> GetCustomerByIdWithAddress(int Id);

		Task<List<Customer>> GetSearchResult(string search);
	}
}
