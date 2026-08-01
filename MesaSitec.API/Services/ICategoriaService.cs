using MesaSitec.API.DTOs;

namespace MesaSitec.API.Services;

public interface ICategoriaService
{
    Task<List<CategoriaResponse>> GetCategoriasAsync(Guid tenantId);
}