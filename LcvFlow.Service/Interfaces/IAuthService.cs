using LcvFlow.Service.Dtos.Auth;

namespace LcvFlow.Service.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> ValidateUserAsync(string username, string password);
}