using MesaSitec.API.DTOs;

namespace MesaSitec.API.Services;

public interface ISolicitudService
{
    Task<SolicitudesResponse> GetSolicitudesAsync(Guid tenantId);
}