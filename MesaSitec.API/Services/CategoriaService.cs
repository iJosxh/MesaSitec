using MesaSitec.API.Data;
using MesaSitec.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MesaSitec.API.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ApplicationDbContext _context;

    public CategoriaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaResponse>> GetCategoriasAsync(Guid tenantId)
    {
        return await _context.Categorias
            .Where(c => c.TenantId == tenantId && c.Activo)
            .OrderBy(c => c.Nombre)
            .Select(c => new CategoriaResponse
            {
                Id = c.Id,
                Nombre = c.Nombre,
                SlaHoras = c.SlaHoras
            })
            .ToListAsync();
    }
}