using Microsoft.EntityFrameworkCore;
using ProjetoInvestimentos.Models;

namespace ProjetoInvestimentos.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Investimento> Investimentos { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Investimento>(entity =>
        {
            entity.ToTable("investimentos", "public");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(50);
            entity.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(20);
            entity.Property(e => e.Valor).HasColumnName("valor").HasColumnType("decimal(18,2)");
            entity.Property(e => e.Operacao).HasColumnName("operacao").HasMaxLength(20);
            entity.Property(e => e.CriadoEm).HasColumnName("criado_em");
            entity.Property(e => e.AlteradoEm).HasColumnName("alterado_em");
            
            // UserCpf não é mapeado para coluna - será calculado via query
            entity.Ignore(e => e.UserCpf);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("user_profiles", "public");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cpf).HasColumnName("cpf").HasMaxLength(11);
            entity.Property(e => e.Nome).HasColumnName("nome");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        base.OnModelCreating(modelBuilder);
    }
}