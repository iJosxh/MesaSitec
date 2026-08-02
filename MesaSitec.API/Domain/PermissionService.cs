using MesaSitec.API.Enums;
using MesaSitec.API.Models;

namespace MesaSitec.API.Domain;

public static class PermissionService
{
    public static bool PuedeListar(
        Usuario usuario,
        Solicitud solicitud)
    {
        if (usuario.Rol == Rol.Admin)
            return true;

        if (usuario.Rol == Rol.Agente)
            return true;

        return solicitud.SolicitanteId == usuario.Id;
    }

    public static bool PuedeVerDetalle(
        Usuario usuario,
        Solicitud solicitud)
    {
        if (usuario.Rol == Rol.Admin)
            return true;

        if (usuario.Rol == Rol.Agente)
            return true;

        return solicitud.SolicitanteId == usuario.Id;
    }

    public static bool PuedeCrear(
        Usuario usuario)
    {
        return true;
    }

    public static bool PuedeEditar(
        Usuario usuario,
        Solicitud solicitud)
    {
        if (usuario.Rol == Rol.Admin)
            return true;

        if (usuario.Rol == Rol.Agente)
            return true;

        return solicitud.SolicitanteId == usuario.Id
            && solicitud.Estado == EstadoSolicitud.Nueva;
    }

    public static bool PuedeAsignar(Usuario usuario)
    {
        return usuario.Rol == Rol.Admin
            || usuario.Rol == Rol.Agente;
    }

    public static bool PuedeIniciar(Usuario usuario)
    {
        return usuario.Rol == Rol.Admin
            || usuario.Rol == Rol.Agente;
    }

    public static bool PuedeResolver(Usuario usuario)
    {
        return usuario.Rol == Rol.Admin
            || usuario.Rol == Rol.Agente;
    }

    public static bool PuedeReabrir(Usuario usuario)
    {
        return usuario.Rol == Rol.Admin
            || usuario.Rol == Rol.Agente;
    }

    public static bool PuedeCerrar(
        Usuario usuario,
        Solicitud solicitud)
    {
        if (usuario.Rol == Rol.Admin)
            return true;

        if (usuario.Rol == Rol.Agente)
            return true;

        return solicitud.SolicitanteId == usuario.Id;
    }

    public static bool PuedeCancelar(
        Usuario usuario)
    {
        return usuario.Rol == Rol.Admin;
    }
}