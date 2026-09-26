using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LocadoraVeiculosErick.Models;

public class Aluguel : IValidatableObject
{
    [Key]
    public int AluguelId { get; set; }

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

    public DateTime DataInicio { get; set; }

    public DateTime DataFimPrevista { get; set; }

    public DateTime? DataDevolucao { get; set; }

    [Range(0, int.MaxValue)]
    public int QuilometragemInicial { get; set; }

    [Range(0, int.MaxValue)]
    public int? QuilometragemFinal { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Range(typeof(decimal), "0.01", "99999999.99", ParseLimitsInInvariantCulture = true)]
    public decimal ValorDiaria { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Range(typeof(decimal), "0", "99999999.99", ParseLimitsInInvariantCulture = true)]
    public decimal ValorTotal { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DataInicio == default || DataFimPrevista == default)
            yield return new ValidationResult("Informe as datas do aluguel.", new[] { nameof(DataInicio), nameof(DataFimPrevista) });
        if (DataFimPrevista <= DataInicio)
            yield return new ValidationResult("A data final prevista deve ser posterior ao início.", new[] { nameof(DataFimPrevista) });
        if (DataDevolucao.HasValue && DataDevolucao.Value < DataInicio)
            yield return new ValidationResult("A devolução não pode anteceder o início.", new[] { nameof(DataDevolucao) });
        if (QuilometragemFinal.HasValue && QuilometragemFinal.Value < QuilometragemInicial)
            yield return new ValidationResult("A quilometragem final não pode ser menor que a inicial.", new[] { nameof(QuilometragemFinal) });
        if (DataDevolucao.HasValue != QuilometragemFinal.HasValue)
            yield return new ValidationResult("Informe devolução e quilometragem final juntas.", new[] { nameof(DataDevolucao), nameof(QuilometragemFinal) });
    }
}
