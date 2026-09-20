using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculosErick.Models;

public class Fabricante
{
    [Key]
    public int FabricanteId { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(60)]
    public string? PaisOrigem { get; set; }

    public ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
}
