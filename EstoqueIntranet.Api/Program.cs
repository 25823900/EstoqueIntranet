using EstoqueIntranet.Api.Data;
using EstoqueIntranet.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("A connection string named 'DefaultConnection' is required.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalDev", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Seed(db);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("LocalDev");

app.MapGet("/api/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }));

app.MapGet("/api/produtos", async (AppDbContext db) =>
    await db.Produtos
        .Include(p => p.UnidadeMedida)
        .Include(p => p.Localizacao)
        .OrderBy(p => p.Nome)
        .Select(p => new
        {
            p.Id,
            p.Codigo,
            p.Nome,
            p.Descricao,
            p.EstoqueAtual,
            p.EstoqueMinimo,
            Unidade = p.UnidadeMedida != null ? new { p.UnidadeMedida.Id, p.UnidadeMedida.Nome, p.UnidadeMedida.Sigla } : null,
            Localizacao = p.Localizacao != null ? new { p.Localizacao.Id, p.Localizacao.Nome, p.Localizacao.Setor } : null
        })
        .ToListAsync());

app.MapGet("/api/produtos/{codigo}", async (string codigo, AppDbContext db) =>
{
    var produto = await db.Produtos
        .Include(p => p.UnidadeMedida)
        .Include(p => p.Localizacao)
        .Include(p => p.QrCodes)
        .FirstOrDefaultAsync(p => p.Codigo == codigo);

    if (produto is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new
    {
        produto.Id,
        produto.Codigo,
        produto.Nome,
        produto.Descricao,
        produto.EstoqueAtual,
        produto.EstoqueMinimo,
        unidade = produto.UnidadeMedida != null ? new { produto.UnidadeMedida.Id, produto.UnidadeMedida.Nome, produto.UnidadeMedida.Sigla } : null,
        localizacao = produto.Localizacao != null ? new { produto.Localizacao.Id, produto.Localizacao.Nome, produto.Localizacao.Setor } : null,
        qrCode = produto.QrCodes.FirstOrDefault()?.Codigo
    });
});

app.Run();
