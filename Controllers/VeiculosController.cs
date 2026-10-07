using LocadoraVeiculosErick.Data;
using LocadoraVeiculosErick.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculosErick.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly ApplicationContext _context;
    public VeiculosController(ApplicationContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Veiculo>>> GetTodos() =>
        Ok(await _context.Veiculos.AsNoTracking().OrderBy(x => x.VeiculoId).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Veiculo>> GetPorId(int id)
    {
        var item = await _context.Veiculos.AsNoTracking().FirstOrDefaultAsync(x => x.VeiculoId == id);
        if (item == null) return NotFound(new { mensagem = "Registro não encontrado." });
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Veiculo>> Post(Veiculo item)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (item.VeiculoId != 0) return BadRequest(new { mensagem = "O ID é gerado pelo banco; envie zero ou omita o campo." });
        item.Placa = item.Placa.Trim().Replace("-", "").ToUpperInvariant();
        if (!await _context.Fabricantes.AnyAsync(f => f.FabricanteId == item.FabricanteId))
            return BadRequest(new { mensagem = "Fabricante inexistente." });
        if (await _context.Veiculos.AnyAsync(v => v.VeiculoId != item.VeiculoId && v.Placa == item.Placa))
            return Conflict(new { mensagem = "Placa já cadastrada." });
        _context.Veiculos.Add(item);
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sql && (sql.Number == 2601 || sql.Number == 2627 || sql.Number == 547))
        {
            return Conflict(new { mensagem = "Não foi possível gravar: registro duplicado ou relacionamento inválido." });
        }
        return CreatedAtAction(nameof(GetPorId), new { id = item.VeiculoId }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, Veiculo item)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (id != item.VeiculoId) return BadRequest(new { mensagem = "O ID da URL deve ser igual ao ID do corpo." });
        var atual = await _context.Veiculos.FindAsync(id);
        if (atual == null) return NotFound(new { mensagem = "Registro não encontrado." });
        item.Placa = item.Placa.Trim().Replace("-", "").ToUpperInvariant();
        if (!await _context.Fabricantes.AnyAsync(f => f.FabricanteId == item.FabricanteId))
            return BadRequest(new { mensagem = "Fabricante inexistente." });
        if (await _context.Veiculos.AnyAsync(v => v.VeiculoId != item.VeiculoId && v.Placa == item.Placa))
            return Conflict(new { mensagem = "Placa já cadastrada." });
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
        var item = await _context.Veiculos.FindAsync(id);
        if (item == null) return NotFound(new { mensagem = "Registro não encontrado." });
        if (await _context.Alugueis.AnyAsync(a => a.VeiculoId == id) ||
            await _context.Reservas.AnyAsync(r => r.VeiculoId == id))
            return Conflict(new { mensagem = "O registro possui vínculos e não pode ser excluído." });
        _context.Veiculos.Remove(item);
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
}
