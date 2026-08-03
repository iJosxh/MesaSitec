using System.ComponentModel.DataAnnotations;

namespace MesaSitec.API.DTOs;

public class EjecutarTransicionRequest
{
    [Required]
    public string Accion { get; set; } = string.Empty;

    public Guid? AgenteId { get; set; }

    public string? Motivo { get; set; }
}