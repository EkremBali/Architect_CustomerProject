using CustomerProject.Entity;
using Microsoft.EntityFrameworkCore;

namespace CustomerProject.Data.Concrete
{
    public class CustomerProjectDbContext : DbContext
    {
        public CustomerProjectDbContext(DbContextOptions<CustomerProjectDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<AddressInformation> AddressInformations { get; set; }
        public DbSet<ContactInformation> ContactInformations { get; set; }
        public DbSet<PhoneCode> PhoneCodes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                        .HasIndex(c => c.TC)
                        .IsUnique();

			modelBuilder.Entity<Customer>()
					    .Property(c => c.TC)
					    .HasMaxLength(11);


			modelBuilder.Entity<Customer>()
                        .Property(c => c.BirthYear)
                        .HasMaxLength(4);

			modelBuilder.Entity<PhoneCode>()
						.Property(c => c.Code)
						.HasMaxLength(10);

		}
    }
}
