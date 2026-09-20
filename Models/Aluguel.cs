using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocadoraVeiculosErick.Models;

public class Aluguel
{
    [Key]
    public int AluguelId { get; set; }

    [ForeignKey(nameof(Cliente))]
    public int ClienteId { get; set; }

    public Cliente? Cliente { get; set; }

    [ForeignKey(nameof(Veiculo))]
    public int VeiculoId { get; set; }

    public Veiculo? Veiculo { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataFimPrevista { get; set; }

    public DateTime? DataDevolucao { get; set; }

    public int QuilometragemInicial { get; set; }

    public int? QuilometragemFinal { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal ValorDiaria { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal ValorTotal { get; set; }
}
