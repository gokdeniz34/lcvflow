using LcvFlow.Service.Dtos.Auth;

namespace LcvFlow.Service;

public class AuthResultDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public UserDto? Data { get; set; }
}
