namespace EstoqueIntranet.Api.Models;

public class UnidadeMedida
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
