using Microsoft.EntityFrameworkCore;
using ViaCidada.Ouvidoria.Api.Models;

namespace ViaCidada.Ouvidoria.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<RegiaoUrbana> RegioesUrbanas => Set<RegiaoUrbana>();
    public DbSet<PropostaMelhoria> PropostasMelhoria => Set<PropostaMelhoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RegiaoUrbana>()
            .HasMany(regiao => regiao.Propostas)
            .WithOne(proposta => proposta.RegiaoUrbana)
            .HasForeignKey(proposta => proposta.RegiaoUrbanaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
