using LcvFlow.Data.Context;
using LcvFlow.Domain;
using LcvFlow.Domain.Entities;
using LcvFlow.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LcvFlow.Data.Repositories;

public class AdminUserRepository : EfRepository<AdminUser>, IAdminUserRepository
{
    public AdminUserRepository(AppDbContext context) : base(context)
    {
    }


    public async Task<AdminUser?> GetByUsernameAsync(string username)
    {
        return await _context.Set<AdminUser>().FirstOrDefaultAsync(x => x.Username == username);
    }
}
