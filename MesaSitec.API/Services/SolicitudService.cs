using MesaSitec.API.Data;
using MesaSitec.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MesaSitec.API.Services;

public class SolicitudService : ISolicitudService
{
    private readonly ApplicationDbContext _context;

    public SolicitudService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SolicitudesResponse> GetSolicitudesAsync(Guid tenantId)
    {
        var solicitudes = await _context.Solicitudes
            .Include(s => s.Categoria)
            .Include(s => s.Agente)
            .Where(s => s.TenantId == tenantId)
            .OrderByDescending(s => s.FechaCreacion)
            .ToListAsync();

        var items = solicitudes.Select(s => new SolicitudResponse
        {
            Id = s.Id,
            Codigo = s.Codigo,
            Titulo = s.Titulo,
            Estado = s.Estado.ToString(),
            Prioridad = s.Prioridad.ToString(),

            Categoria = new CategoriaSimpleResponse
            {
                Id = s.Categoria!.Id,
                Nombre = s.Categoria.Nombre
            },

            Agente = s.Agente == null
                ? null
                : new AgenteSimpleResponse
                {
                    Id = s.Agente.Id,
                    Nombre = s.Agente.Nombre
                },

            FechaCreacion = s.FechaCreacion,
            FechaLimiteSla = s.FechaLimiteSla,
            Vencida = s.FechaLimiteSla < DateTime.UtcNow
        }).ToList();

        return new SolicitudesResponse
        {
            Items = items,
            Page = 1,
            PageSize = items.Count,
            Total = items.Count,
            TotalPaginas = 1
        };
    }
}