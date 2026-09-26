using LocadoraVeiculosErick.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculosErick.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FiltrosController : ControllerBase
{
    private readonly ApplicationContext _context;
    public FiltrosController(ApplicationContext context) => _context = context;

    // INNER JOIN entre veículos e fabricantes.
    [HttpGet("veiculos-por-fabricante/{fabricanteId:int}")]
    public async Task<IActionResult> VeiculosPorFabricante(int fabricanteId)
    {
        if (fabricanteId <= 0) return BadRequest(new { mensagem = "Informe um ID positivo." });
        var consulta = from v in _context.Veiculos
                       join f in _context.Fabricantes on v.FabricanteId equals f.FabricanteId
                       where f.FabricanteId == fabricanteId
                       orderby v.Modelo
                       select new { v.VeiculoId, v.Modelo, v.Placa, v.Disponivel, Fabricante = f.Nome };
        return Ok(await consulta.ToListAsync());
    }

    // INNER JOIN próprio da entidade Reserva do projeto do Erick.
    [HttpGet("reservas-por-cliente/{clienteId:int}")]
    public async Task<IActionResult> ReservasPorCliente(int clienteId)
    {
        if (clienteId <= 0) return BadRequest(new { mensagem = "Informe um ID positivo." });
        var consulta = from r in _context.Reservas
                       join c in _context.Clientes on r.ClienteId equals c.ClienteId
                       join v in _context.Veiculos on r.VeiculoId equals v.VeiculoId
                       where c.ClienteId == clienteId
                       orderby r.DataInicio
                       select new { r.ReservaId, Cliente = c.Nome, v.Modelo, v.Placa, r.DataInicio, r.DataFim, r.Status };
        return Ok(await consulta.ToListAsync());
    }

    [HttpGet("reservas-por-status")]
    public async Task<IActionResult> ReservasPorStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status)) return BadRequest(new { mensagem = "Informe Pendente, Confirmada ou Cancelada." });
        status = status.Trim().ToLowerInvariant();
        if (status is not ("pendente" or "confirmada" or "cancelada"))
            return BadRequest(new { mensagem = "Informe Pendente, Confirmada ou Cancelada." });
        var consulta = from r in _context.Reservas
                       join c in _context.Clientes on r.ClienteId equals c.ClienteId
                       join v in _context.Veiculos on r.VeiculoId equals v.VeiculoId
                       where r.Status.ToLower() == status
                       orderby r.DataInicio
                       select new { r.ReservaId, Cliente = c.Nome, Veiculo = v.Modelo, r.DataInicio, r.DataFim, r.Status };
        return Ok(await consulta.ToListAsync());
    }

    [HttpGet("reservas-por-periodo")]
    public async Task<IActionResult> ReservasPorPeriodo(DateTime? inicio, DateTime? fim)
    {
        if (!inicio.HasValue || !fim.HasValue || inicio > fim)
            return BadRequest(new { mensagem = "Informe início e fim válidos." });
        var consulta = from r in _context.Reservas
                       join v in _context.Veiculos on r.VeiculoId equals v.VeiculoId
                       where r.DataInicio <= fim.Value && r.DataFim >= inicio.Value
                       orderby r.DataInicio
                       select new { r.ReservaId, v.VeiculoId, v.Modelo, r.ClienteId, r.DataInicio, r.DataFim, r.Status };
        return Ok(await consulta.ToListAsync());
    }

    [HttpGet("alugueis-em-aberto")]
    public async Task<IActionResult> AlugueisEmAberto()
    {
        var consulta = from a in _context.Alugueis
                       join c in _context.Clientes on a.ClienteId equals c.ClienteId
                       join v in _context.Veiculos on a.VeiculoId equals v.VeiculoId
                       where a.DataDevolucao == null
                       orderby a.DataInicio
                       select new { a.AluguelId, Cliente = c.Nome, Veiculo = v.Modelo, v.Placa, a.DataInicio, a.DataFimPrevista, a.ValorTotal };
        return Ok(await consulta.ToListAsync());
    }

    [HttpGet("alugueis-por-cliente/{clienteId:int}")]
    public async Task<IActionResult> AlugueisPorCliente(int clienteId)
    {
        if (clienteId <= 0) return BadRequest(new { mensagem = "Informe um ID positivo." });
        var consulta = from a in _context.Alugueis
                       join c in _context.Clientes on a.ClienteId equals c.ClienteId
                       join v in _context.Veiculos on a.VeiculoId equals v.VeiculoId
                       where c.ClienteId == clienteId
                       orderby a.DataInicio
                       select new { a.AluguelId, Cliente = c.Nome, Veiculo = v.Modelo, a.DataInicio, a.DataDevolucao, a.ValorTotal };
        return Ok(await consulta.ToListAsync());
    }

    // LEFT JOIN: mostra também veículos que nunca receberam uma reserva.
    [HttpGet("veiculos-com-reservas")]
    public async Task<IActionResult> VeiculosComReservas()
    {
        var consulta = from v in _context.Veiculos
                       join r in _context.Reservas on v.VeiculoId equals r.VeiculoId into reservas
                       from r in reservas.DefaultIfEmpty()
                       orderby v.VeiculoId, r.ReservaId
                       select new
                       {
                           v.VeiculoId,
                           v.Modelo,
                           v.Placa,
                           ReservaId = (int?)r.ReservaId,
                           StatusReserva = r == null ? null : r.Status,
                           InicioReserva = r == null ? (DateTime?)null : r.DataInicio
                       };
        return Ok(await consulta.ToListAsync());
    }
}
