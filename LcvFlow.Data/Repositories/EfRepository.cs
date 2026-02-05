using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using LcvFlow.Data.Context;
using LcvFlow.Domain.Common;

namespace LcvFlow.Data.Repositories;

public abstract class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected EfRepository(AppDbContext context) => _context = context;

    public IQueryable<T> Query() => _context.Set<T>();

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        => await Query().FirstOrDefaultAsync(predicate);

    public async Task<T?> GetByIdAsync(int id)
        => await Query().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
        => predicate != null ? await Query().Where(predicate).ToListAsync() : await Query().ToListAsync();

    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<T> entities) => await _context.Set<T>().AddRangeAsync(entities);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public void UpdateRange(IEnumerable<T> entities) => _context.Set<T>().UpdateRange(entities);

    public void Delete(T entity) => _context.Set<T>().Remove(entity);

    public void DeleteRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);
}