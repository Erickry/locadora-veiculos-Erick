using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculosErick.Models;

public class Cliente
{
    [Key]
    public int ClienteId { get; set; }

    [Required]
    [StringLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(14)]
    public string CPF { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Telefone { get; set; }

    public ICollection<Aluguel> Alugueis { get; set; } = new List<Aluguel>();

    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
