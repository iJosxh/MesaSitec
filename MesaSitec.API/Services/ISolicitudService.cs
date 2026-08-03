using MesaSitec.API.DTOs;
using MesaSitec.API.Enums;

namespace MesaSitec.API.Services;

public interface ISolicitudService
{
    Task<SolicitudesResponse> GetSolicitudesAsync(
    Guid tenantId,
    SolicitudesQuery query);

    Task<SolicitudDetalleResponse> GetByIdAsync(
    Guid solicitudId,
    Guid tenantId,
    Guid usuarioId,
    Rol rol);

    Task<SolicitudDetalleResponse> CreateAsync(
    CreateSolicitudRequest request,
    Guid tenantId,
    Guid solicitanteId);

    Task<SolicitudDetalleResponse> UpdateAsync(
    Guid solicitudId,
    UpdateSolicitudRequest request,
    Guid tenantId,
    Guid usuarioId,
    Rol rol);

    Task<SolicitudDetalleResponse> EjecutarTransicionAsync(
    Guid solicitudId,
    EjecutarTransicionRequest request,
    Guid tenantId,
    Guid usuarioId,
    Rol rol);
}