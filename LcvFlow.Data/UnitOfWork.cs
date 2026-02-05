using LcvFlow.Data.Context;
using LcvFlow.Data.Repositories;
using LcvFlow.Domain;
using LcvFlow.Domain.Common;
using LcvFlow.Domain.Interfaces;

namespace LcvFlow.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IGuestRepository Guests { get; }
    public IEventRepository Events { get; }
    public IAdminUserRepository AdminUsers { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        Guests = new GuestRepository(_context);
        Events = new EventRepository(_context);
        AdminUsers = new AdminUserRepository(_context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}