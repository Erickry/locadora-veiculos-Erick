using LocadoraVeiculosErick.Data;
using LocadoraVeiculosErick.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculosErick.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservasController : ControllerBase
{
    private readonly ApplicationContext _context;
    public ReservasController(ApplicationContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reserva>>> GetTodos() =>
        Ok(await _context.Reservas.AsNoTracking().OrderBy(x => x.ReservaId).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Reserva>> GetPorId(int id)
    {
        var item = await _context.Reservas.AsNoTracking().FirstOrDefaultAsync(x => x.ReservaId == id);
        if (item == null) return NotFound(new { mensagem = "Registro não encontrado." });
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Reserva>> Post(Reserva item)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (item.ReservaId != 0) return BadRequest(new { mensagem = "O ID é gerado pelo banco; envie zero ou omita o campo." });
        item.Status = NormalizarStatus(item.Status);
        if (!await _context.Clientes.AnyAsync(c => c.ClienteId == item.ClienteId))
            return BadRequest(new { mensagem = "Cliente inexistente." });
        if (!await _context.Veiculos.AnyAsync(v => v.VeiculoId == item.VeiculoId))
            return BadRequest(new { mensagem = "Veículo inexistente." });
        if (await ExisteConflitoDePeriodo(item))
            return Conflict(new { mensagem = "O veículo já possui uma reserva ativa nesse período." });

        _context.Reservas.Add(item);
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sql && (sql.Number == 2601 || sql.Number == 2627 || sql.Number == 547))
        {
            return Conflict(new { mensagem = "Não foi possível gravar: registro duplicado ou relacionamento inválido." });
        }
        return CreatedAtAction(nameof(GetPorId), new { id = item.ReservaId }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, Reserva item)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (id != item.ReservaId) return BadRequest(new { mensagem = "O ID da URL deve ser igual ao ID do corpo." });
        var atual = await _context.Reservas.FindAsync(id);
        if (atual == null) return NotFound(new { mensagem = "Registro não encontrado." });
        item.Status = NormalizarStatus(item.Status);
        if (!await _context.Clientes.AnyAsync(c => c.ClienteId == item.ClienteId))
            return BadRequest(new { mensagem = "Cliente inexistente." });
        if (!await _context.Veiculos.AnyAsync(v => v.VeiculoId == item.VeiculoId))
            return BadRequest(new { mensagem = "Veículo inexistente." });
        if (await ExisteConflitoDePeriodo(item))
            return Conflict(new { mensagem = "O veículo já possui uma reserva ativa nesse período." });

        _context.Entry(atual).CurrentValues.SetValues(item);
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { mensagem = "O registro foi removido durante a atualização. Consulte novamente." });
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sql && (sql.Number == 2601 || sql.Number == 2627 || sql.Number == 547))
        {
            return Conflict(new { mensagem = "Não foi possível atualizar: registro duplicado ou relacionamento inválido." });
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Reservas.FindAsync(id);
        if (item == null) return NotFound(new { mensagem = "Registro não encontrado." });
        _context.Reservas.Remove(item);
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { mensagem = "O registro foi removido por outra operação. Consulte novamente." });
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sql && sql.Number == 547)
        {
            return Conflict(new { mensagem = "O registro possui vínculos e não pode ser excluído." });
        }
        return NoContent();
    }

    private async Task<bool> ExisteConflitoDePeriodo(Reserva item) =>
        item.Status != "Cancelada" && await _context.Reservas.AnyAsync(r =>
            r.ReservaId != item.ReservaId && r.VeiculoId == item.VeiculoId &&
            r.Status != "Cancelada" && item.DataInicio < r.DataFim && item.DataFim > r.DataInicio);

    private static string NormalizarStatus(string status)
    {
        status = status.Trim().ToLowerInvariant();
        return status switch
        {
            "pendente" => "Pendente",
            "confirmada" => "Confirmada",
            "cancelada" => "Cancelada",
            _ => status
        };
    }
}
