using MesaSitec.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MesaSitec.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<Categoria> Categorias => Set<Categoria>();

    public DbSet<Solicitud> Solicitudes => Set<Solicitud>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId);

        modelBuilder.Entity<Categoria>()
            .HasOne(c => c.Tenant)
            .WithMany()
            .HasForeignKey(c => c.TenantId);

        modelBuilder.Entity<Solicitud>()
            .HasOne(s => s.Tenant)
            .WithMany()
            .HasForeignKey(s => s.TenantId);

        modelBuilder.Entity<Solicitud>()
            .HasOne(s => s.Categoria)
            .WithMany()
            .HasForeignKey(s => s.CategoriaId);

        modelBuilder.Entity<Solicitud>()
            .HasOne(s => s.Solicitante)
            .WithMany()
            .HasForeignKey(s => s.SolicitanteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Solicitud>()
            .HasOne(s => s.Agente)
            .WithMany()
            .HasForeignKey(s => s.AgenteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}