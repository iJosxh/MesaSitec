using MesaSitec.API.DTOs;

namespace MesaSitec.API.Services;

public interface IMeService
{
    Task<UsuarioResponse?> GetMeAsync(Guid userId);
}