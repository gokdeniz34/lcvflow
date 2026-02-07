using System.Linq.Expressions;

namespace LcvFlow.Domain.Common;

public interface IQueryBuilder<T> where T : BaseEntity
{
    IQueryable<T> Query();
}