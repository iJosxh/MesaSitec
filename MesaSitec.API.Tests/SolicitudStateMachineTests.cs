using FluentAssertions;
using MesaSitec.API.Domain;
using MesaSitec.API.Enums;

namespace MesaSitec.API.Tests;

public class SolicitudStateMachineTests
{
    [Fact]
    public void Nueva_Puede_Asignar()
    {
        var resultado = SolicitudStateMachine.PuedeTransicionar(
            EstadoSolicitud.Nueva,
            "asignar");

        resultado.Should().BeTrue();
    }

    [Fact]
    public void Nueva_Puede_Cancelar()
    {
        var resultado = SolicitudStateMachine.PuedeTransicionar(
            EstadoSolicitud.Nueva,
            "cancelar");

        resultado.Should().BeTrue();
    }

    [Fact]
    public void Nueva_No_Puede_Resolver()
    {
        var resultado = SolicitudStateMachine.PuedeTransicionar(
            EstadoSolicitud.Nueva,
            "resolver");

        resultado.Should().BeFalse();
    }

    [Fact]
    public void Asignada_Puede_Iniciar()
    {
        var nuevoEstado = SolicitudStateMachine.ObtenerNuevoEstado(
            EstadoSolicitud.Asignada,
            "iniciar");

        nuevoEstado.Should().Be(EstadoSolicitud.EnProceso);
    }

    [Fact]
    public void EnProceso_Puede_Resolver()
    {
        var nuevoEstado = SolicitudStateMachine.ObtenerNuevoEstado(
            EstadoSolicitud.EnProceso,
            "resolver");

        nuevoEstado.Should().Be(EstadoSolicitud.Resuelta);
    }

    [Fact]
    public void Resuelta_Puede_Cerrar()
    {
        var nuevoEstado = SolicitudStateMachine.ObtenerNuevoEstado(
            EstadoSolicitud.Resuelta,
            "cerrar");

        nuevoEstado.Should().Be(EstadoSolicitud.Cerrada);
    }

    [Fact]
    public void Resuelta_Puede_Reabrir()
    {
        var nuevoEstado = SolicitudStateMachine.ObtenerNuevoEstado(
            EstadoSolicitud.Resuelta,
            "reabrir");

        nuevoEstado.Should().Be(EstadoSolicitud.EnProceso);
    }

    [Fact]
    public void Cerrada_No_Acepta_Transiciones()
    {
        Action accion = () =>
            SolicitudStateMachine.ObtenerNuevoEstado(
                EstadoSolicitud.Cerrada,
                "resolver");

        accion.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("TRANSICION_INVALIDA");
    }

    [Fact]
    public void Cancelada_No_Acepta_Transiciones()
    {
        Action accion = () =>
            SolicitudStateMachine.ObtenerNuevoEstado(
                EstadoSolicitud.Cancelada,
                "asignar");

        accion.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("TRANSICION_INVALIDA");
    }
}