using MesaSitec.API.DTOs;

namespace MesaSitec.API.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}