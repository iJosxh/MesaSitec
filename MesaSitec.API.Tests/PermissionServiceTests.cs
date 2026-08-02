using FluentAssertions;
using MesaSitec.API.Domain;
using MesaSitec.API.Enums;
using MesaSitec.API.Models;

namespace MesaSitec.API.Tests;

public class PermissionServiceTests
{
    private static Usuario CrearUsuario(Rol rol)
    {
        return new Usuario
        {
            Id = Guid.NewGuid(),
            Rol = rol
        };
    }

    private static Solicitud CrearSolicitud(Guid solicitanteId, EstadoSolicitud estado)
    {
        return new Solicitud
        {
            SolicitanteId = solicitanteId,
            Estado = estado
        };
    }

    [Fact]
    public void Admin_Puede_Cancelar()
    {
        var admin = CrearUsuario(Rol.Admin);

        PermissionService.PuedeCancelar(admin)
            .Should().BeTrue();
    }

    [Fact]
    public void Agente_No_Puede_Cancelar()
    {
        var agente = CrearUsuario(Rol.Agente);

        PermissionService.PuedeCancelar(agente)
            .Should().BeFalse();
    }

    [Fact]
    public void Solicitante_No_Puede_Asignar()
    {
        var usuario = CrearUsuario(Rol.Solicitante);

        PermissionService.PuedeAsignar(usuario)
            .Should().BeFalse();
    }

    [Fact]
    public void Admin_Puede_Asignar()
    {
        var admin = CrearUsuario(Rol.Admin);

        PermissionService.PuedeAsignar(admin)
            .Should().BeTrue();
    }

    [Fact]
    public void Agente_Puede_Resolver()
    {
        var agente = CrearUsuario(Rol.Agente);

        PermissionService.PuedeResolver(agente)
            .Should().BeTrue();
    }

    [Fact]
    public void Solicitante_Puede_Editar_Solo_Si_Es_Suya_Y_Nueva()
    {
        var usuario = CrearUsuario(Rol.Solicitante);

        var solicitud = CrearSolicitud(
            usuario.Id,
            EstadoSolicitud.Nueva);

        PermissionService.PuedeEditar(usuario, solicitud)
            .Should().BeTrue();
    }

    [Fact]
    public void Solicitante_No_Puede_Editar_Si_No_Es_Suya()
    {
        var usuario = CrearUsuario(Rol.Solicitante);

        var solicitud = CrearSolicitud(
            Guid.NewGuid(),
            EstadoSolicitud.Nueva);

        PermissionService.PuedeEditar(usuario, solicitud)
            .Should().BeFalse();
    }

    [Fact]
    public void Solicitante_No_Puede_Editar_Si_No_Esta_Nueva()
    {
        var usuario = CrearUsuario(Rol.Solicitante);

        var solicitud = CrearSolicitud(
            usuario.Id,
            EstadoSolicitud.EnProceso);

        PermissionService.PuedeEditar(usuario, solicitud)
            .Should().BeFalse();
    }

    [Fact]
    public void Solicitante_Puede_Ver_Solo_Sus_Solicitudes()
    {
        var usuario = CrearUsuario(Rol.Solicitante);

        var solicitud = CrearSolicitud(
            usuario.Id,
            EstadoSolicitud.Nueva);

        PermissionService.PuedeVerDetalle(usuario, solicitud)
            .Should().BeTrue();
    }

    [Fact]
    public void Solicitante_No_Puede_Ver_Solicitudes_De_Otro()
    {
        var usuario = CrearUsuario(Rol.Solicitante);

        var solicitud = CrearSolicitud(
            Guid.NewGuid(),
            EstadoSolicitud.Nueva);

        PermissionService.PuedeVerDetalle(usuario, solicitud)
            .Should().BeFalse();
    }
}