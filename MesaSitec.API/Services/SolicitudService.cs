using MesaSitec.API.Data;
using MesaSitec.API.DTOs;
using MesaSitec.API.Models;
using MesaSitec.API.Enums;
using MesaSitec.API.Exceptions;
using MesaSitec.API.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MesaSitec.API.Services;

public class SolicitudService : ISolicitudService
{
    private readonly ApplicationDbContext _context;

    public SolicitudService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SolicitudesResponse> GetSolicitudesAsync(
        Guid tenantId, 
        SolicitudesQuery query)
    {
        var consulta = _context.Solicitudes
            .Include(s => s.Categoria)
            .Include(s => s.Agente)
            .Where(s => s.TenantId == tenantId)
            .AsQueryable();

        if (query.Estado.HasValue)
        {
            consulta = consulta.Where(s => s.Estado == query.Estado.Value);
        }

        if (query.Prioridad.HasValue)
        {
            consulta = consulta.Where(s => s.Prioridad == query.Prioridad.Value);
        }

        if (query.CategoriaId.HasValue)
        {
            consulta = consulta.Where(s => s.CategoriaId == query.CategoriaId.Value);
        }

        if (query.AgenteId.HasValue)
        {
            consulta = consulta.Where(s => s.AgenteId == query.AgenteId.Value);
        }   

        if (!string.IsNullOrWhiteSpace(query.Q))
        {
            var texto = query.Q.Trim().ToLower();

            consulta = consulta.Where(s =>
                s.Titulo.ToLower().Contains(texto) ||
                s.Descripcion.ToLower().Contains(texto) ||
                s.Codigo.ToLower().Contains(texto));
        } 

        if (query.Vencidas == true)
        {
            consulta = consulta.Where(s =>
                s.FechaLimiteSla < DateTime.UtcNow &&
                s.Estado != EstadoSolicitud.Resuelta &&
                s.Estado != EstadoSolicitud.Cerrada &&
                s.Estado != EstadoSolicitud.Cancelada);
        }

        if (query.Page < 1)
        {
            throw new BusinessException(
                StatusCodes.Status400BadRequest,
                "PARAMETRO_INVALIDO",
                "Parámetro inválido",
                "El parámetro 'page' debe ser mayor o igual a 1.");
        }

        if (query.PageSize < 1 || query.PageSize > 100)
        {
            throw new BusinessException(
                StatusCodes.Status400BadRequest,
                "PARAMETRO_INVALIDO",
                "Parámetro inválido",
                "El parámetro 'pageSize' debe estar entre 1 y 100.");
        }

        consulta = query.Sort switch
        {
            "fechaCreacion" => consulta.OrderBy(s => s.FechaCreacion),

            "-fechaCreacion" => consulta.OrderByDescending(s => s.FechaCreacion),

            "prioridad" => consulta.OrderBy(s => s.Prioridad),

            "-prioridad" => consulta.OrderByDescending(s => s.Prioridad),

            _ => consulta.OrderByDescending(s => s.FechaCreacion)
        };

        var total = await consulta.CountAsync();

        var solicitudes = await consulta
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
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
            Page = query.Page,
            PageSize = query.PageSize,
            Total = total,
            TotalPaginas = (int)Math.Ceiling((double)total / query.PageSize)
        };
    }

    public async Task<SolicitudDetalleResponse> GetByIdAsync(
    Guid solicitudId,
    Guid tenantId,
    Guid usuarioId,
    Rol rol)
    {
        var solicitud = await _context.Solicitudes
            .Include(s => s.Categoria)
            .Include(s => s.Solicitante)
            .Include(s => s.Agente)
            .FirstOrDefaultAsync(s =>
                s.Id == solicitudId &&
                s.TenantId == tenantId);

        if (solicitud is null)
        {
            throw new BusinessException(
                StatusCodes.Status404NotFound,
                "RECURSO_NO_ENCONTRADO",
                "Solicitud no encontrada",
                "La solicitud no existe o no pertenece a su organización.");
        }

        if (rol == Rol.Solicitante &&
            solicitud.SolicitanteId != usuarioId)
        {
            throw new BusinessException(
                StatusCodes.Status403Forbidden,
                "OPERACION_NO_PERMITIDA",
                "Operación no permitida",
                "No puede consultar solicitudes de otros usuarios.");
        }

        return MapToDetalleResponse(solicitud);
    }

    public async Task<SolicitudDetalleResponse> CreateAsync(
    CreateSolicitudRequest request,
    Guid tenantId,
    Guid solicitanteId)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c =>
                c.Id == request.CategoriaId &&
                c.TenantId == tenantId &&
                c.Activo);       

        if (categoria is null)
        {
            throw new BusinessException(
                StatusCodes.Status404NotFound,
                "RECURSO_NO_ENCONTRADO",
                "Categoría no encontrada",
                "La categoría indicada no existe.");
        }

        var siguienteNumero = await _context.Solicitudes.CountAsync() + 1;

        var codigo = $"SOL-2026-{siguienteNumero:00000}"; 

        var solicitante = await _context.Usuarios
            .FirstOrDefaultAsync(u =>
                u.Id == solicitanteId &&
                u.TenantId == tenantId);

        if (solicitante is null)
        {
            throw new BusinessException(
                StatusCodes.Status404NotFound,
                "RECURSO_NO_ENCONTRADO",
                "Usuario no encontrado",
                "El solicitante no existe.");
        }

        var fechaCreacion = DateTime.UtcNow;

        var fechaLimiteSla = SlaCalculator.CalcularFechaLimite(
            fechaCreacion,
            categoria.SlaHoras,
            request.Prioridad);

        var solicitud = new Solicitud
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Codigo = codigo,
            Titulo = request.Titulo,
            Descripcion = request.Descripcion,
            CategoriaId = categoria.Id,
            Prioridad = request.Prioridad,
            Estado = EstadoSolicitud.Nueva,
            SolicitanteId = solicitante.Id,
            FechaCreacion = fechaCreacion,
            FechaLimiteSla = fechaLimiteSla
        };

        _context.Solicitudes.Add(solicitud);
        await _context.SaveChangesAsync();   

        solicitud = await _context.Solicitudes
        .Include(s => s.Categoria)
        .Include(s => s.Solicitante)
        .FirstAsync(s => s.Id == solicitud.Id); 

        return MapToDetalleResponse(solicitud);
    }

    public async Task<SolicitudDetalleResponse> UpdateAsync(
    Guid solicitudId,
    UpdateSolicitudRequest request,
    Guid tenantId,
    Guid usuarioId,
    Rol rol)
    {
        // 1. Buscar la solicitud
        var solicitud = await _context.Solicitudes
            .Include(s => s.Categoria)
            .Include(s => s.Solicitante)
            .Include(s => s.Agente)
            .FirstOrDefaultAsync(s =>
                s.Id == solicitudId &&
                s.TenantId == tenantId);

        if (solicitud is null)
        {
            throw new BusinessException(
                StatusCodes.Status404NotFound,
                "RECURSO_NO_ENCONTRADO",
                "Solicitud no encontrada",
                "La solicitud no existe o no pertenece a su organización.");
        }

        // 2. RN-03 (permisos del solicitante)
        if (rol == Rol.Solicitante)
        {
            if (solicitud.SolicitanteId != usuarioId)
            {
                throw new BusinessException(
                    StatusCodes.Status403Forbidden,
                    "OPERACION_NO_PERMITIDA",
                    "Operación no permitida",
                    "No puede modificar solicitudes de otros usuarios.");
            }

            if (solicitud.Estado != EstadoSolicitud.Nueva)
            {
                throw new BusinessException(
                    StatusCodes.Status403Forbidden,
                    "OPERACION_NO_PERMITIDA",
                    "Operación no permitida",
                    "Solo puede editar solicitudes en estado Nueva.");
            }
        }

        // 3. Buscar la nueva categoría
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c =>
                c.Id == request.CategoriaId &&
                c.TenantId == tenantId &&
                c.Activo);

        if (categoria is null)
        {
            throw new BusinessException(
                StatusCodes.Status404NotFound,
                "RECURSO_NO_ENCONTRADO",
                "Categoría no encontrada",
                "La categoría indicada no existe.");
        }

        solicitud.Titulo = request.Titulo;
        solicitud.Descripcion = request.Descripcion;
        solicitud.CategoriaId = categoria.Id;
        solicitud.Categoria = categoria;
        solicitud.Prioridad = request.Prioridad;
        solicitud.FechaLimiteSla = SlaCalculator.CalcularFechaLimite(
        solicitud.FechaCreacion,
        categoria.SlaHoras,
        request.Prioridad);

        await _context.SaveChangesAsync();

        return MapToDetalleResponse(solicitud);
    }

    public async Task<SolicitudDetalleResponse> EjecutarTransicionAsync(
    Guid solicitudId,
    EjecutarTransicionRequest request,
    Guid tenantId,
    Guid usuarioId,
    Rol rol)
    {
        var solicitud = await _context.Solicitudes
            .Include(s => s.Categoria)
            .Include(s => s.Solicitante)
            .Include(s => s.Agente)
            .FirstOrDefaultAsync(s =>
                s.Id == solicitudId &&
                s.TenantId == tenantId);

        if (solicitud is null)
        {
            throw new BusinessException(
                StatusCodes.Status404NotFound,
                "RECURSO_NO_ENCONTRADO",
                "Solicitud no encontrada",
                "La solicitud no existe o no pertenece a su organización.");
        }

        if (rol == Rol.Solicitante)
        {
            if (solicitud.SolicitanteId != usuarioId)
            {
                throw new BusinessException(
                    StatusCodes.Status403Forbidden,
                    "OPERACION_NO_PERMITIDA",
                    "Operación no permitida",
                    "No puede realizar acciones sobre solicitudes de otros usuarios.");
            }

            if (request.Accion != "cancelar")
            {
                throw new BusinessException(
                    StatusCodes.Status403Forbidden,
                    "OPERACION_NO_PERMITIDA",
                    "Operación no permitida",
                    "El solicitante solo puede cancelar sus solicitudes.");
            }
        }

        if (!SolicitudStateMachine.PuedeTransicionar(
        solicitud.Estado,
        request.Accion))
        {
            throw new BusinessException(
                StatusCodes.Status422UnprocessableEntity,
                "TRANSICION_INVALIDA",
                "Transición inválida",
                "La transición solicitada no es válida para el estado actual.");
        }

        var nuevoEstado = SolicitudStateMachine.ObtenerNuevoEstado(
            solicitud.Estado,
            request.Accion);

            if (request.Accion == "asignar")
            {
                if (request.AgenteId is null)
                {
                    throw new BusinessException(
                        StatusCodes.Status422UnprocessableEntity,
                        "AGENTE_REQUERIDO",
                        "Agente requerido",
                        "Debe indicar el agente que atenderá la solicitud.");
                }

                var agente = await _context.Usuarios
                    .FirstOrDefaultAsync(u =>
                        u.Id == request.AgenteId &&
                        u.TenantId == tenantId &&
                        u.Rol == Rol.Agente &&
                        u.Activo);

                if (agente is null)
                {
                    throw new BusinessException(
                        StatusCodes.Status422UnprocessableEntity,
                        "AGENTE_INVALIDO",
                        "Agente inválido",
                        "El agente indicado no existe o no pertenece a la organización.");
                }

                solicitud.AgenteId = agente.Id;
                solicitud.Agente = agente;
            }

        if ((request.Accion == "resolver" ||
            request.Accion == "cancelar") &&
            string.IsNullOrWhiteSpace(request.Motivo))
        {
            throw new BusinessException(
                StatusCodes.Status422UnprocessableEntity,
                "MOTIVO_REQUERIDO",
                "Motivo requerido",
                "Debe indicar el motivo de la operación.");
        }    

        solicitud.Estado = nuevoEstado;

        switch (request.Accion)
        {
            case "resolver":
                solicitud.FechaResolucion = DateTime.UtcNow;
                solicitud.MotivoResolucion = request.Motivo;
                break;

            case "cancelar":
                solicitud.MotivoCancelacion = request.Motivo;
                break;

            case "reabrir":
                solicitud.FechaResolucion = null;
                solicitud.MotivoResolucion = null;
                break;
        }

        await _context.SaveChangesAsync();

        return MapToDetalleResponse(solicitud);
    }

    private SolicitudDetalleResponse MapToDetalleResponse(Solicitud solicitud)
    {
        return new SolicitudDetalleResponse
        {
            Id = solicitud.Id,
            Codigo = solicitud.Codigo,
            Titulo = solicitud.Titulo,
            Descripcion = solicitud.Descripcion,
            Estado = solicitud.Estado.ToString(),
            Prioridad = solicitud.Prioridad.ToString(),
            Categoria = new CategoriaResponse
            {
                Id = solicitud.Categoria!.Id,
                Nombre = solicitud.Categoria.Nombre,
                SlaHoras = solicitud.Categoria.SlaHoras
            },
            Solicitante = new UsuarioSimpleResponse
            {
                Id = solicitud.Solicitante!.Id,
                Nombre = solicitud.Solicitante.Nombre
            },

            Agente = solicitud.Agente == null
                ? null
                : new UsuarioSimpleResponse
                {
                    Id = solicitud.Agente.Id,
                    Nombre = solicitud.Agente.Nombre
                },

            FechaCreacion = solicitud.FechaCreacion,
            FechaLimiteSla = solicitud.FechaLimiteSla,
            FechaResolucion = solicitud.FechaResolucion,
            MotivoResolucion = solicitud.MotivoResolucion,
            MotivoCancelacion = solicitud.MotivoCancelacion,

            Vencida =
                solicitud.FechaLimiteSla < DateTime.UtcNow &&
                solicitud.Estado != EstadoSolicitud.Resuelta &&
                solicitud.Estado != EstadoSolicitud.Cerrada &&
                solicitud.Estado != EstadoSolicitud.Cancelada
        };
    }
}