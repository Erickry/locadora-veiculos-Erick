using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
    [RegularExpression(@"\d{11}|\d{3}\.\d{3}\.\d{3}-\d{2}", ErrorMessage = "Informe o CPF com 11 dígitos ou no formato 000.000.000-00.")]
    public string CPF { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Telefone { get; set; }

    [JsonIgnore]
    [ValidateNever]
    public ICollection<Aluguel> Alugueis { get; set; } = new List<Aluguel>();

    [JsonIgnore]
    [ValidateNever]
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
