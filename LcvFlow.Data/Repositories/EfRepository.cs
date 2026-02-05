using LcvFlow.Data.Context;
using LcvFlow.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace LcvFlow.Data.Repositories;

public abstract class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    public EfRepository(AppDbContext context)
    {
        _context = context;
    }
    public T GetById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid ID");
        }

        return _context.Set<T>().Find(id) ?? throw new KeyNotFoundException($"Entity with ID {id} not found.");
    }
    public List<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }
    public void Add(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
        }

        _context.Set<T>().Add(entity);
        _context.SaveChanges();
    }
    public void Update(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
        }

        _context.Set<T>().Update(entity);
        _context.SaveChanges();
    }
    public void Delete(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
        }

        _context.Set<T>().Remove(entity);
        _context.SaveChanges();
    }
    public void Remove(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
        }

        _context.Set<T>().Remove(entity);
        _context.SaveChanges();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }
    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity); await _context.SaveChangesAsync();
    }
    public async Task RemoveAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}
