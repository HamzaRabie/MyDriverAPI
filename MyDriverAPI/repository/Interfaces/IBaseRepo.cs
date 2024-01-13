using System.Linq.Expressions;

namespace MyDriver.repository.Interfaces
{
    public interface IBaseRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByAppIDAsync(Expression<Func<T,bool>> match);
        Task<IEnumerable<T>> GetAllWithInclude(Expression<Func<T,bool>> match , string[] includes=null);
        Task<T> GetOneWithInclude(Expression<Func<T, bool>> match, string[] includes=null);

        Task<T> AddAsync(T entity);
        Task<T> DeleteAsync(int id);
    }
}
