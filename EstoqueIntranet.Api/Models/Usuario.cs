namespace EstoqueIntranet.Api.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string NomeUsuario { get; set; } = string.Empty;
    public string Perfil { get; set; } = "Operador";
    public bool Ativo { get; set; } = true;
    public ICollection<MovimentoEstoque> Movimentos { get; set; } = new List<MovimentoEstoque>();
}
