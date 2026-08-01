namespace MesaSitec.API.DTOs;

public class CategoriaResponse
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int SlaHoras { get; set; }
}