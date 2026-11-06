namespace EstoqueIntranet.Api.Models;

public class Produto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int UnidadeMedidaId { get; set; }
    public UnidadeMedida? UnidadeMedida { get; set; }
    public int? LocalizacaoId { get; set; }
    public Localizacao? Localizacao { get; set; }
    public decimal EstoqueMinimo { get; set; }
    public decimal EstoqueAtual { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime? DataCadastro { get; set; } = DateTime.UtcNow;
    public ICollection<MovimentoEstoque> Movimentos { get; set; } = new List<MovimentoEstoque>();
    public ICollection<QrCodeProduto> QrCodes { get; set; } = new List<QrCodeProduto>();
}
