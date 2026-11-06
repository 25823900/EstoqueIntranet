namespace EstoqueIntranet.Api.Models;

public class QrCodeProduto
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public DateTime DataGeracao { get; set; } = DateTime.UtcNow;
    public bool Ativo { get; set; } = true;
}
