namespace EstoqueIntranet.Api.Models;

public class MovimentoEstoque
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }
    public string Tipo { get; set; } = string.Empty; // Entrada, Saida, Ajuste, Transferencia, Devolucao
    public decimal Quantidade { get; set; }
    public int UnidadeMedidaId { get; set; }
    public UnidadeMedida? UnidadeMedida { get; set; }
    public int? UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
    public string? Observacao { get; set; }
}
