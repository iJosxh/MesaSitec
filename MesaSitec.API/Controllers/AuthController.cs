using MesaSitec.API.DTOs;
using MesaSitec.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace MesaSitec.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        if (response is null)
        {
            return Unauthorized(new
            {
                message = "Credenciales inválidas."
            });
        }

        return Ok(response);
    }
}