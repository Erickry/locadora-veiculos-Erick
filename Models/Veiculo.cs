using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocadoraVeiculosErick.Models;

public class Veiculo
{
    [Key]
    public int VeiculoId { get; set; }

    [Required]
    [StringLength(100)]
    public string Modelo { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string Placa { get; set; } = string.Empty;

    public int AnoFabricacao { get; set; }

    public int Quilometragem { get; set; }

    public bool Disponivel { get; set; } = true;

    [ForeignKey(nameof(Fabricante))]
    public int FabricanteId { get; set; }

    public Fabricante? Fabricante { get; set; }

    public ICollection<Aluguel> Alugueis { get; set; } = new List<Aluguel>();

    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
