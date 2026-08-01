using MesaSitec.API.Enums;

namespace MesaSitec.API.Models;

public class Usuario
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public Rol Rol { get; set; }

    public bool Activo { get; set; }

    public Tenant? Tenant { get; set; }
}