using LcvFlow.Data.Context;
using LcvFlow.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace LcvFlow.Data.Repositories;

public abstract class EfRepository<T>(
    AppDbContext context)
    : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context = context;

    public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);
    public async Task<List<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
    public async Task AddAsync(T entity) { await _context.Set<T>().AddAsync(entity); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(T entity) { _context.Set<T>().Update(entity); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(T entity) { _context.Set<T>().Remove(entity); await _context.SaveChangesAsync(); }
}