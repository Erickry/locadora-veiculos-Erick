using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LocadoraVeiculosErick.Models;

public class Reserva : IValidatableObject
{
    [Key]
    public int ReservaId { get; set; }

    [ForeignKey(nameof(Cliente))]
    [Range(1, int.MaxValue)]
    public int ClienteId { get; set; }

    [JsonIgnore]
    [ValidateNever]
    public Cliente? Cliente { get; set; }

    [ForeignKey(nameof(Veiculo))]
    [Range(1, int.MaxValue)]
    public int VeiculoId { get; set; }

    [JsonIgnore]
    [ValidateNever]
    public Veiculo? Veiculo { get; set; }

    public DateTime DataReserva { get; set; } = DateTime.Now;

    public DateTime DataInicio { get; set; }

    public DateTime DataFim { get; set; }

    [Required]
    [StringLength(20)]
    [RegularExpression("Pendente|Confirmada|Cancelada", ErrorMessage = "Use Pendente, Confirmada ou Cancelada.")]
    public string Status { get; set; } = "Pendente";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DataInicio == default || DataFim == default)
            yield return new ValidationResult("Informe início e fim da reserva.", new[] { nameof(DataInicio), nameof(DataFim) });
        if (DataFim <= DataInicio)
            yield return new ValidationResult("O fim deve ser posterior ao início.", new[] { nameof(DataFim) });
        if (DataReserva == default)
            yield return new ValidationResult("Informe a data da reserva.", new[] { nameof(DataReserva) });
    }
}
