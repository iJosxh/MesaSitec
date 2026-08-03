using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MesaSitec.API.DTOs;
using MesaSitec.API.Enums;
using MesaSitec.API.Services;
using MesaSitec.API.Exceptions;
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
    public async Task<ActionResult<SolicitudesResponse>> GetSolicitudes(
        [FromQuery] SolicitudesQuery query)
    {
        var tenantIdClaim = User.FindFirstValue("tenantId");

        if (tenantIdClaim is null)
            return Unauthorized();

        var tenantId = Guid.Parse(tenantIdClaim);

        var resultado = await _solicitudService.GetSolicitudesAsync(
            tenantId,
            query);

        return Ok(resultado);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SolicitudDetalleResponse>> GetById(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var tenantId = User.FindFirstValue("tenantId");

        var rol = User.FindFirstValue("rol");

        if (userId is null ||
            tenantId is null ||
            rol is null)
        {
            return Unauthorized();
        }

        var solicitud = await _solicitudService.GetByIdAsync(
            id,
            Guid.Parse(tenantId),
            Guid.Parse(userId),
            Enum.Parse<Rol>(rol));

        return Ok(solicitud);
    }

    [HttpPost]
    public async Task<ActionResult<SolicitudDetalleResponse>> Create(
        CreateSolicitudRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var tenantId = User.FindFirstValue("tenantId");

        if (userId is null || tenantId is null)
        {
            return Unauthorized();
        }

        var solicitud = await _solicitudService.CreateAsync(
            request,
            Guid.Parse(tenantId),
            Guid.Parse(userId));

        return CreatedAtAction(
            nameof(GetById),
            new { id = solicitud.Id },
            solicitud);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SolicitudDetalleResponse>> Update(
        Guid id,
        UpdateSolicitudRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var tenantId = User.FindFirstValue("tenantId");
        var rol = User.FindFirstValue("rol");

        if (userId is null ||
            tenantId is null ||
            rol is null)
        {
            return Unauthorized();
        }

        var solicitud = await _solicitudService.UpdateAsync(
            id,
            request,
            Guid.Parse(tenantId),
            Guid.Parse(userId),
            Enum.Parse<Rol>(rol));

        return Ok(solicitud);
    }

    [HttpPost("{id:guid}/transiciones")]
    public async Task<ActionResult<SolicitudDetalleResponse>> EjecutarTransicion(
        Guid id,
        [FromBody] EjecutarTransicionRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var tenantId = User.FindFirstValue("tenantId");

        var rol = User.FindFirstValue("rol");

        if (userId is null ||
            tenantId is null ||
            rol is null)
        {
            return Unauthorized();
        }

        var solicitud = await _solicitudService.EjecutarTransicionAsync(
            id,
            request,
            Guid.Parse(tenantId),
            Guid.Parse(userId),
            Enum.Parse<Rol>(rol));

        return Ok(solicitud);
    }
}