namespace MesaSitec.API.DTOs;

public class SolicitudResponse
{
    public Guid Id { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string Prioridad { get; set; } = string.Empty;

    public CategoriaSimpleResponse Categoria { get; set; } = new();

    public AgenteSimpleResponse? Agente { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaLimiteSla { get; set; }

    public bool Vencida { get; set; }
}