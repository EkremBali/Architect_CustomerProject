namespace CustomerProject.Data.Abstract
{
    //Bütün repository'lerin ortak yapacağı işlemlerin bulunduğu temel interface.
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
