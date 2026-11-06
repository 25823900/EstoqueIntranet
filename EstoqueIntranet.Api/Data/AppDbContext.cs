using EstoqueIntranet.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueIntranet.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UnidadeMedida> UnidadesMedida => Set<UnidadeMedida>();
    public DbSet<Localizacao> Localizacoes => Set<Localizacao>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<MovimentoEstoque> MovimentosEstoque => Set<MovimentoEstoque>();
    public DbSet<QrCodeProduto> QrCodes => Set<QrCodeProduto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UnidadeMedida>(entity =>
        {
            entity.HasIndex(u => u.Sigla).IsUnique();
            entity.Property(u => u.Nome).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Sigla).IsRequired().HasMaxLength(20);
        });

        modelBuilder.Entity<Localizacao>(entity =>
        {
            entity.Property(l => l.Nome).IsRequired().HasMaxLength(120);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(u => u.NomeUsuario).IsUnique();
            entity.Property(u => u.Nome).IsRequired().HasMaxLength(120);
            entity.Property(u => u.NomeUsuario).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasIndex(p => p.Codigo).IsUnique();
            entity.Property(p => p.Codigo).IsRequired().HasMaxLength(50);
            entity.Property(p => p.Nome).IsRequired().HasMaxLength(150);
            entity.Property(p => p.EstoqueMinimo).HasColumnType("decimal(18,3)");
            entity.Property(p => p.EstoqueAtual).HasColumnType("decimal(18,3)");

            entity.HasOne(p => p.UnidadeMedida)
                .WithMany(u => u.Produtos)
                .HasForeignKey(p => p.UnidadeMedidaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Localizacao)
                .WithMany(l => l.Produtos)
                .HasForeignKey(p => p.LocalizacaoId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<MovimentoEstoque>(entity =>
        {
            entity.Property(m => m.Tipo).IsRequired().HasMaxLength(30);
            entity.Property(m => m.Quantidade).HasColumnType("decimal(18,3)");

            entity.HasOne(m => m.Produto)
                .WithMany(p => p.Movimentos)
                .HasForeignKey(m => m.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(m => m.UnidadeMedida)
                .WithMany()
                .HasForeignKey(m => m.UnidadeMedidaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Usuario)
                .WithMany(u => u.Movimentos)
                .HasForeignKey(m => m.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<QrCodeProduto>(entity =>
        {
            entity.HasIndex(q => q.Codigo).IsUnique();
            entity.Property(q => q.Codigo).IsRequired().HasMaxLength(200);

            entity.HasOne(q => q.Produto)
                .WithMany(p => p.QrCodes)
                .HasForeignKey(q => q.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
