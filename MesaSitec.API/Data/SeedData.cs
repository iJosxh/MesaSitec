using MesaSitec.API.Enums;
using MesaSitec.API.Models;
using Microsoft.AspNetCore.Identity;

namespace MesaSitec.API.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Tenants.Any())
            return;

        var tenants = SeedTenants(context);

        SeedUsuarios(context, tenants);

        SeedCategorias(context, tenants);

        context.SaveChanges();
    }

    private static Dictionary<string, Tenant> SeedTenants(ApplicationDbContext context)
    {
        var norte = new Tenant
        {
            Id = Guid.NewGuid(),
            Nombre = "Cooperativa Norte",
            Activo = true
        };

        var sur = new Tenant
        {
            Id = Guid.NewGuid(),
            Nombre = "Bufete Sur",
            Activo = true
        };

        context.Tenants.AddRange(norte, sur);

        return new Dictionary<string, Tenant>
        {
            { "NORTE", norte },
            { "SUR", sur }
        };
    }

    private static void SeedUsuarios(ApplicationDbContext context, Dictionary<string, Tenant> tenants)
    {
        var hasher = new PasswordHasher<Usuario>();

        void CrearUsuario(string email, string nombre, Rol rol, Tenant tenant)
        {
            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                Email = email,
                Nombre = nombre,
                Rol = rol,
                Activo = true
            };

            usuario.PasswordHash = hasher.HashPassword(usuario, "Sitec.2026");

            context.Usuarios.Add(usuario);
        }

        var norte = tenants["NORTE"];
        var sur = tenants["SUR"];

        CrearUsuario("admin@norte.test", "Administrador Norte", Rol.Admin, norte);
        CrearUsuario("agente1@norte.test", "Agente Norte 1", Rol.Agente, norte);
        CrearUsuario("agente2@norte.test", "Agente Norte 2", Rol.Agente, norte);
        CrearUsuario("user1@norte.test", "Usuario Norte 1", Rol.Solicitante, norte);
        CrearUsuario("user2@norte.test", "Usuario Norte 2", Rol.Solicitante, norte);

        CrearUsuario("admin@sur.test", "Administrador Sur", Rol.Admin, sur);
        CrearUsuario("user1@sur.test", "Usuario Sur 1", Rol.Solicitante, sur);
    }

    private static void SeedCategorias(ApplicationDbContext context, Dictionary<string, Tenant> tenants)
    {
        void CrearCategoria(string nombre, int slaHoras, Tenant tenant)
        {
            context.Categorias.Add(new Categoria
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                Nombre = nombre,
                SlaHoras = slaHoras,
                Activo = true
            });
        }

        foreach (var tenant in tenants.Values)
        {
            CrearCategoria("Incidente", 8, tenant);
            CrearCategoria("Requerimiento", 40, tenant);
            CrearCategoria("Consulta", 24, tenant);
            CrearCategoria("Falla crítica", 4, tenant);
        }
    }
}