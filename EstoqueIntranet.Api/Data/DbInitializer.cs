using EstoqueIntranet.Api.Models;

namespace EstoqueIntranet.Api.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.UnidadesMedida.Any())
        {
            return;
        }

        var unidades = new[]
        {
            new UnidadeMedida { Nome = "Quilograma", Sigla = "KG" },
            new UnidadeMedida { Nome = "Grama", Sigla = "G" },
            new UnidadeMedida { Nome = "Litro", Sigla = "LT" },
            new UnidadeMedida { Nome = "Mililitro", Sigla = "ML" },
            new UnidadeMedida { Nome = "Unidade", Sigla = "UN" },
            new UnidadeMedida { Nome = "Caixa", Sigla = "CX" }
        };

        context.UnidadesMedida.AddRange(unidades);
        context.SaveChanges();

        var localizacao = new Localizacao { Nome = "Almoxarifado Principal", Setor = "A1" };
        context.Localizacoes.Add(localizacao);
        context.SaveChanges();

        var usuarios = new[]
        {
            new Usuario { Nome = "Administrador", NomeUsuario = "admin", Perfil = "Administrador" },
            new Usuario { Nome = "Operador 1", NomeUsuario = "operador1", Perfil = "Operador" }
        };

        context.Usuarios.AddRange(usuarios);
        context.SaveChanges();

        var materialKg = context.UnidadesMedida.First(u => u.Sigla == "KG");
        var materialUn = context.UnidadesMedida.First(u => u.Sigla == "UN");
        var materialLt = context.UnidadesMedida.First(u => u.Sigla == "LT");

        var produtos = new[]
        {
            new Produto
            {
                Codigo = "B1147B",
                Nome = "Filme 200 mm",
                Descricao = "Filme para embalagem",
                UnidadeMedidaId = materialKg.Id,
                LocalizacaoId = localizacao.Id,
                EstoqueMinimo = 50,
                EstoqueAtual = 276,
                Ativo = true
            },
            new Produto
            {
                Codigo = "B5017",
                Nome = "Ribbon 110mm x 450mm",
                Descricao = "Ribbon para impressora",
                UnidadeMedidaId = materialUn.Id,
                LocalizacaoId = localizacao.Id,
                EstoqueMinimo = 100,
                EstoqueAtual = 700,
                Ativo = true
            },
            new Produto
            {
                Codigo = "B0901",
                Nome = "Hexano NRISCO83",
                Descricao = "Solvente químico",
                UnidadeMedidaId = materialLt.Id,
                LocalizacaoId = localizacao.Id,
                EstoqueMinimo = 10,
                EstoqueAtual = 180,
                Ativo = true
            }
        };

        context.Produtos.AddRange(produtos);
        context.SaveChanges();

        context.QrCodes.AddRange(
            new QrCodeProduto { ProdutoId = produtos[0].Id, Codigo = "QR-B1147B-001" },
            new QrCodeProduto { ProdutoId = produtos[1].Id, Codigo = "QR-B5017-001" },
            new QrCodeProduto { ProdutoId = produtos[2].Id, Codigo = "QR-B0901-001" });

        context.SaveChanges();
    }
}
