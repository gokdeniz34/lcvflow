using LcvFlow.Domain.Common;
using LcvFlow.Domain.Entities;

namespace LcvFlow.Domain.Interfaces;

public interface IGuestRepository : IRepository<Guest>
{
    Task<Guest?> GetByTokenAsync(string token);
}