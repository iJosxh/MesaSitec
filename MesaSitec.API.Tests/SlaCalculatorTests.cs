using FluentAssertions;
using MesaSitec.API.Domain;
using MesaSitec.API.Enums;

namespace MesaSitec.API.Tests;

public class SlaCalculatorTests
{
    [Fact]
    public void Critica_De_8_Horas_Debe_Dar_4_Horas()
    {
        var fecha = new DateTime(2026, 1, 15, 8, 0, 0);

        var limite = SlaCalculator.CalcularFechaLimite(
            fecha,
            8,
            Prioridad.Critica);

        limite.Should().Be(fecha.AddHours(4));
    }

    [Fact]
    public void Alta_De_8_Horas_Debe_Dar_6_Horas()
    {
        var fecha = new DateTime(2026, 1, 15, 8, 0, 0);

        var limite = SlaCalculator.CalcularFechaLimite(
            fecha,
            8,
            Prioridad.Alta);

        limite.Should().Be(fecha.AddHours(6));
    }

    [Fact]
    public void Media_De_24_Horas_Debe_Dar_24_Horas()
    {
        var fecha = new DateTime(2026, 1, 15, 8, 0, 0);

        var limite = SlaCalculator.CalcularFechaLimite(
            fecha,
            24,
            Prioridad.Media);

        limite.Should().Be(fecha.AddHours(24));
    }

    [Fact]
    public void Baja_De_24_Horas_Debe_Dar_48_Horas()
    {
        var fecha = new DateTime(2026, 1, 15, 8, 0, 0);

        var limite = SlaCalculator.CalcularFechaLimite(
            fecha,
            24,
            Prioridad.Baja);

        limite.Should().Be(fecha.AddHours(48));
    }

    [Fact]
    public void Solicitud_Nueva_Vencida_Debe_Retornar_True()
    {
        var vencida = SlaCalculator.EstaVencida(
            DateTime.UtcNow.AddHours(-1),
            EstadoSolicitud.Nueva,
            DateTime.UtcNow);

        vencida.Should().BeTrue();
    }

    [Fact]
    public void Solicitud_EnProceso_Vencida_Debe_Retornar_True()
    {
        var vencida = SlaCalculator.EstaVencida(
            DateTime.UtcNow.AddHours(-1),
            EstadoSolicitud.EnProceso,
            DateTime.UtcNow);

        vencida.Should().BeTrue();
    }

    [Fact]
    public void Solicitud_Resuelta_Nunca_Esta_Vencida()
    {
        var vencida = SlaCalculator.EstaVencida(
            DateTime.UtcNow.AddHours(-100),
            EstadoSolicitud.Resuelta,
            DateTime.UtcNow);

        vencida.Should().BeFalse();
    }

    [Fact]
    public void Solicitud_Cerrada_Nunca_Esta_Vencida()
    {
        var vencida = SlaCalculator.EstaVencida(
            DateTime.UtcNow.AddHours(-100),
            EstadoSolicitud.Cerrada,
            DateTime.UtcNow);

        vencida.Should().BeFalse();
    }

    [Fact]
    public void Solicitud_Cancelada_Nunca_Esta_Vencida()
    {
        var vencida = SlaCalculator.EstaVencida(
            DateTime.UtcNow.AddHours(-100),
            EstadoSolicitud.Cancelada,
            DateTime.UtcNow);

        vencida.Should().BeFalse();
    }
}