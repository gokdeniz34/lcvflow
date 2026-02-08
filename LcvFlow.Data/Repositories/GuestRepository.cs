using LcvFlow.Data.Context;
using LcvFlow.Domain.Entities;
using LcvFlow.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LcvFlow.Data.Repositories;

public class GuestRepository : EfRepository<Guest>, IGuestRepository
{
    public GuestRepository(AppDbContext context) : base(context) { }

    public async Task<Guest?> GetByTokenAsync(string token)
    {
        var guest = await _context.Guests.FirstOrDefaultAsync(x => x.AccessToken == token);
        guest?.LoadAdditionalProperties(); // Veriyi çeker çekmez sözlüğü dolduruyoruz
        return guest;
    }
}