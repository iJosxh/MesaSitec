using MesaSitec.API.DTOs;
using MesaSitec.API.Services;

using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MesaSitec.API.Controllers;

[ApiController]
[Route("me")]
[Authorize]
public class MeController : ControllerBase
{
    private readonly IMeService _meService;

    public MeController(IMeService meService)
    {
        _meService = meService;
    }

    [HttpGet]
    public async Task<ActionResult<UsuarioResponse>> GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return Unauthorized();

        var usuario = await _meService.GetMeAsync(Guid.Parse(userId));

        if (usuario is null)
            return NotFound();

        return Ok(usuario);
    }
}