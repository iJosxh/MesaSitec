using MesaSitec.API.Enums;

namespace MesaSitec.API.Domain;

public static class SolicitudStateMachine
{
    public static bool PuedeTransicionar(
        EstadoSolicitud estadoActual,
        string accion)
    {
        accion = accion.ToLowerInvariant();

        return estadoActual switch
        {
            EstadoSolicitud.Nueva =>
                accion is "asignar" or "cancelar",

            EstadoSolicitud.Asignada =>
                accion is "iniciar"
                    or "asignar"
                    or "cancelar",

            EstadoSolicitud.EnProceso =>
                accion is "resolver"
                    or "asignar"
                    or "cancelar",

            EstadoSolicitud.Resuelta =>
                accion is "cerrar"
                    or "reabrir",

            EstadoSolicitud.Cerrada => false,

            EstadoSolicitud.Cancelada => false,

            _ => false
        };
    }

    public static EstadoSolicitud ObtenerNuevoEstado(
        EstadoSolicitud estadoActual,
        string accion)
    {
        accion = accion.ToLowerInvariant();

        if (!PuedeTransicionar(estadoActual, accion))
        {
            throw new InvalidOperationException(
                "TRANSICION_INVALIDA");
        }

        return (estadoActual, accion) switch
        {
            (EstadoSolicitud.Nueva, "asignar")
                => EstadoSolicitud.Asignada,

            (EstadoSolicitud.Nueva, "cancelar")
                => EstadoSolicitud.Cancelada,

            (EstadoSolicitud.Asignada, "iniciar")
                => EstadoSolicitud.EnProceso,

            (EstadoSolicitud.Asignada, "asignar")
                => EstadoSolicitud.Asignada,

            (EstadoSolicitud.Asignada, "cancelar")
                => EstadoSolicitud.Cancelada,

            (EstadoSolicitud.EnProceso, "resolver")
                => EstadoSolicitud.Resuelta,

            (EstadoSolicitud.EnProceso, "asignar")
                => EstadoSolicitud.Asignada,

            (EstadoSolicitud.EnProceso, "cancelar")
                => EstadoSolicitud.Cancelada,

            (EstadoSolicitud.Resuelta, "cerrar")
                => EstadoSolicitud.Cerrada,

            (EstadoSolicitud.Resuelta, "reabrir")
                => EstadoSolicitud.EnProceso,

            _ => throw new InvalidOperationException(
                "TRANSICION_INVALIDA")
        };
    }
}