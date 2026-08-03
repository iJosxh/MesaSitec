using System.Security.Claims;
using MesaSitec.API.DTOs;
using MesaSitec.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MesaSitec.API.Controllers;

[ApiController]
[Route("api/v1/categorias")]
[Authorize]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoriaResponse>>> GetCategorias()
    {
        var tenantIdClaim = User.FindFirstValue("tenantId");

        if (tenantIdClaim is null)
            return Unauthorized();

        var tenantId = Guid.Parse(tenantIdClaim);

        var categorias = await _categoriaService.GetCategoriasAsync(tenantId);

        return Ok(categorias);
    }
}