using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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

    [JsonIgnore]
    [ValidateNever]
    public ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
}
