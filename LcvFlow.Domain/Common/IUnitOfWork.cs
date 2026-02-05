using LcvFlow.Domain.Interfaces;

namespace LcvFlow.Domain.Common;

public interface IUnitOfWork : IDisposable
{
    IGuestRepository Guests { get; }
    IEventRepository Events { get; }
    IAdminUserRepository AdminUsers { get; }
    
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}