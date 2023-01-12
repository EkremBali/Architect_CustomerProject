using Azure;
using Microsoft.EntityFrameworkCore;
using CustomerProject.Data.Abstract;
using CustomerProject.Entity;

namespace CustomerProject.Data.Concrete
{
    public class EFCoreAddressRepository : EFCoreGenericRepository<AddressInformation>, IAddressRepository
	{
        public EFCoreAddressRepository(CustomerProjectDbContext context) : base(context)
        {

        }

        private CustomerProjectDbContext Context { get { return context as CustomerProjectDbContext; } }

	}
}
