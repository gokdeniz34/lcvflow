using LcvFlow.Data.Context;
using LcvFlow.Domain;
using LcvFlow.Domain.Guests;
using Microsoft.EntityFrameworkCore;

namespace LcvFlow.Data.Repositories;

public class GuestRepository : EfRepository<Guest>, IGuestRepository
{
    public GuestRepository(AppDbContext context) : base(context) { }

    public async Task<Guest?> GetByTokenAsync(string token)
    {
        return await _context.Guests.FirstOrDefaultAsync(x => x.AccessToken == token);
    }
}