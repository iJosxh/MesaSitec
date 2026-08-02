using MesaSitec.API.Enums;
using MesaSitec.API.Models;
using MesaSitec.API.Domain;
using Microsoft.AspNetCore.Identity;

namespace MesaSitec.API.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Tenants.Any())
            return;

        var tenants = SeedTenants(context);
        var fechaBase = GetSeedBaseDate();

        SeedUsuarios(context, tenants);
        SeedCategorias(context, tenants);

        // Guardar primero tenants, usuarios y categorías
        context.SaveChanges();

        // Ahora sí existen en la BD
        SeedSolicitudes(context, fechaBase);

        // Guardar las solicitudes
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

    private static void SeedSolicitudes(
    ApplicationDbContext context,
    DateTime fechaBase)
    {
        if (context.Solicitudes.Any())
            return;

        var tenantNorte = context.Tenants
            .First(t => t.Nombre == "Cooperativa Norte");

        var tenantSur = context.Tenants
            .First(t => t.Nombre == "Bufete Sur");

        var categoriasNorte = context.Categorias
            .Where(c => c.TenantId == tenantNorte.Id)
            .ToList();

        var categoriasSur = context.Categorias
            .Where(c => c.TenantId == tenantSur.Id)
            .ToList();

        var solicitantesNorte = context.Usuarios
            .Where(u => u.TenantId == tenantNorte.Id &&
                        u.Rol == Rol.Solicitante)
            .ToList();

        var solicitantesSur = context.Usuarios
            .Where(u => u.TenantId == tenantSur.Id &&
                        u.Rol == Rol.Solicitante)
            .ToList();

        var agentesNorte = context.Usuarios
            .Where(u => u.TenantId == tenantNorte.Id &&
                        u.Rol == Rol.Agente)
            .ToList();

        var estados = new[]
        {
            EstadoSolicitud.Nueva,
            EstadoSolicitud.Asignada,
            EstadoSolicitud.EnProceso,
            EstadoSolicitud.Resuelta,
            EstadoSolicitud.Cerrada,
            EstadoSolicitud.Cancelada
        };

        var prioridades = new[]
        {
            Prioridad.Baja,
            Prioridad.Media,
            Prioridad.Alta,
            Prioridad.Critica
        };

        // -------------------------
        // Cooperativa Norte (25)
        // -------------------------

        for (int i = 1; i <= 25; i++)
        {
            var estado = estados[(i - 1) % estados.Length];
            var prioridad = prioridades[(i - 1) % prioridades.Length];

            CrearSolicitud(
                context,
                tenantNorte,
                solicitantesNorte[(i - 1) % solicitantesNorte.Count],
                estado == EstadoSolicitud.Nueva
                    ? null
                    : agentesNorte[(i - 1) % agentesNorte.Count],
                categoriasNorte[(i - 1) % categoriasNorte.Count],
                i,
                estado,
                prioridad,
                fechaBase);
        }

        // -------------------------
        // Bufete Sur (8)
        // -------------------------

        for (int i = 26; i <= 33; i++)
        {
            var estado = estados[(i - 1) % estados.Length];
            var prioridad = prioridades[(i - 1) % prioridades.Length];

            CrearSolicitud(
                context,
                tenantSur,
                solicitantesSur[(i - 26) % solicitantesSur.Count],
                null,
                categoriasSur[(i - 26) % categoriasSur.Count],
                i,
                estado,
                prioridad,
                fechaBase);
        }
    }

    private static void CrearSolicitud(
    ApplicationDbContext context,
    Tenant tenant,
    Usuario solicitante,
    Usuario? agente,
    Categoria categoria,
    int numero,
    EstadoSolicitud estado,
    Prioridad prioridad,
    DateTime fechaBase)
    {
        // Todas las fechas se calculan respecto a SEED_FECHA_BASE
        var fechaCreacion = fechaBase.AddDays(-numero);

        var fechaLimite = SlaCalculator.CalcularFechaLimite(
        fechaCreacion,
        categoria.SlaHoras,
        prioridad);

        // Las primeras 5 solicitudes quedan vencidas
        if (numero <= 5)
        {
            fechaLimite = fechaBase.AddDays(-1);
        }

        DateTime? fechaResolucion = null;

        // Las solicitudes resueltas y cerradas tienen fecha de resolución
        if (estado == EstadoSolicitud.Resuelta ||
            estado == EstadoSolicitud.Cerrada)
        {
            fechaResolucion = fechaCreacion.AddHours(categoria.SlaHoras / 2.0);
        }

        context.Solicitudes.Add(new Solicitud
        {
            Id = Guid.NewGuid(),

            TenantId = tenant.Id,

            Codigo = $"SOL-2026-{numero:00000}",

            Titulo = $"Solicitud #{numero}",

            Descripcion = $"Solicitud generada automáticamente #{numero}",

            CategoriaId = categoria.Id,

            Prioridad = prioridad,

            Estado = estado,

            SolicitanteId = solicitante.Id,

            AgenteId = agente?.Id,

            FechaCreacion = fechaCreacion,

            FechaLimiteSla = fechaLimite,

            FechaResolucion = fechaResolucion,

            Tenant = tenant,

            Categoria = categoria,

            Solicitante = solicitante,

            Agente = agente
        });
    }

    private static DateTime GetSeedBaseDate()
    {
        var value = Environment.GetEnvironmentVariable("SEED_FECHA_BASE");

        if (string.IsNullOrWhiteSpace(value))
        {
            value = "2026-01-15T08:00:00Z";
        }

        return DateTime.Parse(
            value,
            null,
            System.Globalization.DateTimeStyles.AdjustToUniversal);
    }
}