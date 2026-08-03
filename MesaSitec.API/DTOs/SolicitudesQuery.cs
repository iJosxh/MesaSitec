using MesaSitec.API.Enums;

namespace MesaSitec.API.DTOs;

public class SolicitudesQuery
{
    public EstadoSolicitud? Estado { get; set; }

    public Prioridad? Prioridad { get; set; }

    public Guid? CategoriaId { get; set; }

    public Guid? AgenteId { get; set; }

    public string? Q { get; set; }

    public bool? Vencidas { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public string Sort { get; set; } = "-fechaCreacion";
}