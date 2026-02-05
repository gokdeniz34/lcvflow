using LcvFlow.Domain.Common;
using LcvFlow.Domain.Entities;

namespace LcvFlow.Domain.Interfaces;

public interface IAdminUserRepository : IRepository<AdminUser>
{
    Task<AdminUser?> GetByUsernameAsync(string username);
}
