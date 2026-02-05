using System.Security.Claims;
using AutoMapper;
using LcvFlow.Domain.Interfaces;
using LcvFlow.Service.Dtos.Auth;
using LcvFlow.Service.Helpers.Auth;
using LcvFlow.Service.Interfaces;

namespace LcvFlow.Service.Concretes;

public class AuthService : IAuthService
{
    private readonly IAdminUserRepository _adminUserRepository;
    private readonly IMapper _mapper;
    public AuthService(IAdminUserRepository adminUserRepository, IMapper mapper)
    {
        _adminUserRepository = adminUserRepository;
        _mapper = mapper;
    }

    public async Task<AuthResultDto> ValidateUserAsync(string username, string password)
    {
        var user = await _adminUserRepository.GetByUsernameAsync(username);

        if (user == null || !PasswordHelper.Verify(user.PasswordHash,password))
        {
            return new AuthResultDto { IsSuccess = false, Message = "Hatalı giriş!" };
        }

        return new AuthResultDto
        {
            IsSuccess = true,
            Data = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<UserDto> GetUser(string username)
    {
        var userEntity = await _adminUserRepository.GetByUsernameAsync(username);
        return _mapper.Map<UserDto>(userEntity);
    }
}