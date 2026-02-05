using System.Linq.Expressions;

namespace LcvFlow.Domain.Common;

public interface IRepository<T> where T : BaseEntity
{
    IQueryable<T> Query();
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}