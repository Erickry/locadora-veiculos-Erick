using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
    [RegularExpression(@"[A-Za-z]{3}-?[0-9][A-Za-z0-9][0-9]{2}", ErrorMessage = "Informe uma placa válida.")]
    public string Placa { get; set; } = string.Empty;

    [Range(1886, 2100)]
    public int AnoFabricacao { get; set; }

    [Range(0, int.MaxValue)]
    public int Quilometragem { get; set; }

    public bool Disponivel { get; set; } = true;

    [ForeignKey(nameof(Fabricante))]
    [Range(1, int.MaxValue)]
    public int FabricanteId { get; set; }

    [JsonIgnore]
    [ValidateNever]
    public Fabricante? Fabricante { get; set; }

    [JsonIgnore]
    [ValidateNever]
    public ICollection<Aluguel> Alugueis { get; set; } = new List<Aluguel>();

    [JsonIgnore]
    [ValidateNever]
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
