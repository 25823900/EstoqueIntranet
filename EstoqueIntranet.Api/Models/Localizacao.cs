namespace EstoqueIntranet.Api.Models;

public class Localizacao
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Setor { get; set; }
    public bool Ativo { get; set; } = true;
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
