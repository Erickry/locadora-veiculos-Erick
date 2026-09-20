using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocadoraVeiculosErick.Models;

public class Reserva
{
    [Key]
    public int ReservaId { get; set; }

    [ForeignKey(nameof(Cliente))]
    public int ClienteId { get; set; }

    public Cliente? Cliente { get; set; }

    [ForeignKey(nameof(Veiculo))]
    public int VeiculoId { get; set; }

    public Veiculo? Veiculo { get; set; }

    public DateTime DataReserva { get; set; } = DateTime.Now;

    public DateTime DataInicio { get; set; }

    public DateTime DataFim { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Pendente";
}
