using System.Linq.Expressions;
using LcvFlow.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace LcvFlow.Data.Repositories;

public class EfQueryBuilder<T> : IQueryBuilder<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet;

    public EfQueryBuilder(DbSet<T> dbSet)
    {
        _dbSet = dbSet;
    }

    public IQueryable<T> Query() => _dbSet.AsQueryable();
}