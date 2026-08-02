using MesaSitec.API.Enums;

namespace MesaSitec.API.Domain;

public static class SlaCalculator
{
    public static DateTime CalcularFechaLimite(
        DateTime fechaCreacion,
        int slaHoras,
        Prioridad prioridad)
    {
        var factor = prioridad switch
        {
            Prioridad.Critica => 0.5,
            Prioridad.Alta => 0.75,
            Prioridad.Media => 1.0,
            Prioridad.Baja => 2.0,
            _ => 1.0
        };

        var horas = slaHoras * factor;

        return fechaCreacion.AddHours(horas);
    }

    public static bool EstaVencida(
        DateTime fechaLimite,
        EstadoSolicitud estado,
        DateTime fechaActual)
    {
        if (estado is EstadoSolicitud.Resuelta
            or EstadoSolicitud.Cerrada
            or EstadoSolicitud.Cancelada)
        {
            return false;
        }

        return fechaLimite < fechaActual;
    }
}