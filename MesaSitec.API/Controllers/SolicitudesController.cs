using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MesaSitec.API.DTOs;
using MesaSitec.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MesaSitec.API.Controllers;

[ApiController]
[Route("api/v1/solicitudes")]
[Authorize]
public class SolicitudesController : ControllerBase
{
    private readonly ISolicitudService _solicitudService;

    public SolicitudesController(ISolicitudService solicitudService)
    {
        _solicitudService = solicitudService;
    }

    [HttpGet]
    public async Task<ActionResult<SolicitudesResponse>> GetSolicitudes()
    {
        var tenantIdClaim = User.FindFirstValue("tenantId");

        if (tenantIdClaim is null)
            return Unauthorized();

        var tenantId = Guid.Parse(tenantIdClaim);

        var resultado = await _solicitudService.GetSolicitudesAsync(tenantId);

        return Ok(resultado);
    }
}