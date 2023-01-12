using Microsoft.EntityFrameworkCore;
using CustomerProject.Data.Abstract;
using CustomerProject.Entity;

namespace CustomerProject.Data.Concrete
{
    //Bütün entity'lerin crud işlemlerinin yapıldığı generic sınıf.
    public class EFCoreGenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext context;
        public EFCoreGenericRepository(DbContext context)
        {
            this.context = context;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
			return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task CreateAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
