using MesaSitec.API.Data;
using MesaSitec.API.DTOs;
using MesaSitec.API.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace MesaSitec.API.Services;

public class MeService : IMeService
{
    private readonly ApplicationDbContext _context;

    public MeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UsuarioResponse> GetMeAsync(Guid userId)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Tenant)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (usuario is null)
        {
            throw new BusinessException(
                StatusCodes.Status404NotFound,
                "RECURSO_NO_ENCONTRADO",
                "Recurso no encontrado",
                "El usuario no existe.");
        }

        return new UsuarioResponse
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol.ToString(),
            TenantId = usuario.TenantId,
            TenantNombre = usuario.Tenant!.Nombre
        };
    }
}