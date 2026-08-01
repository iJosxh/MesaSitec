namespace MesaSitec.API.DTOs;

public class SolicitudesResponse
{
    public List<SolicitudResponse> Items { get; set; } = new();

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int Total { get; set; }

    public int TotalPaginas { get; set; }
}