using Microsoft.EntityFrameworkCore.Query;

using System.Linq.Expressions;

namespace TMS.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<T> GetByIdAsync(object id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task UpdateAsync(T entity);
        Task AddAsync(T entity);
        Task AddRange(IEnumerable<T> entities);

        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task DeleteAsync(object id);
        Task DeleteAsync(T entity);
        Task SaveAsync();
        Task RemoveAllAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        Task RemoveRangeAsync(IEnumerable<T> entities);




    }
}
