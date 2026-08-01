using MesaSitec.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MesaSitec.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TenantsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var tenants = await _context.Tenants
            .AsNoTracking()
            .ToListAsync();

        return Ok(tenants);
    }
}