using LocadoraVeiculosErick.Models;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculosErick.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<Fabricante> Fabricantes { get; set; } = null!;
    public DbSet<Veiculo> Veiculos { get; set; } = null!;
    public DbSet<Cliente> Clientes { get; set; } = null!;
    public DbSet<Aluguel> Alugueis { get; set; } = null!;
    public DbSet<Reserva> Reservas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Veiculo>()
            .HasIndex(veiculo => veiculo.Placa)
            .IsUnique();

        modelBuilder.Entity<Cliente>()
            .HasIndex(cliente => cliente.CPF)
            .IsUnique();

        modelBuilder.Entity<Cliente>()
            .HasIndex(cliente => cliente.Email)
            .IsUnique();

        modelBuilder.Entity<Veiculo>()
            .HasOne(veiculo => veiculo.Fabricante)
            .WithMany(fabricante => fabricante.Veiculos)
            .HasForeignKey(veiculo => veiculo.FabricanteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Aluguel>()
            .HasOne(aluguel => aluguel.Cliente)
            .WithMany(cliente => cliente.Alugueis)
            .HasForeignKey(aluguel => aluguel.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Aluguel>()
            .HasOne(aluguel => aluguel.Veiculo)
            .WithMany(veiculo => veiculo.Alugueis)
            .HasForeignKey(aluguel => aluguel.VeiculoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reserva>()
            .HasOne(reserva => reserva.Cliente)
            .WithMany(cliente => cliente.Reservas)
            .HasForeignKey(reserva => reserva.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reserva>()
            .HasOne(reserva => reserva.Veiculo)
            .WithMany(veiculo => veiculo.Reservas)
            .HasForeignKey(reserva => reserva.VeiculoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
