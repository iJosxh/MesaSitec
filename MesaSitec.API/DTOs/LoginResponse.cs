namespace MesaSitec.API.DTOs;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public int ExpiraEn { get; set; }

    public UsuarioResponse Usuario { get; set; } = new();
}