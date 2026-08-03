using System.ComponentModel.DataAnnotations;
using MesaSitec.API.Enums;

namespace MesaSitec.API.DTOs;

public class CreateSolicitudRequest
{
    [Required]
    [MinLength(5)]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [MinLength(10)]
    [MaxLength(5000)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    public Guid CategoriaId { get; set; }

    [Required]
    public Prioridad Prioridad { get; set; }
}