using LocadoraVeiculosErick.Data;
using LocadoraVeiculosErick.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculosErick.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FabricantesController : ControllerBase
{
    private readonly ApplicationContext _context;
    public FabricantesController(ApplicationContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Fabricante>>> GetTodos() =>
        Ok(await _context.Fabricantes.AsNoTracking().OrderBy(x => x.FabricanteId).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Fabricante>> GetPorId(int id)
    {
        var item = await _context.Fabricantes.AsNoTracking().FirstOrDefaultAsync(x => x.FabricanteId == id);
        if (item == null) return NotFound(new { mensagem = "Registro não encontrado." });
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Fabricante>> Post(Fabricante item)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (item.FabricanteId != 0) return BadRequest(new { mensagem = "O ID é gerado pelo banco; envie zero ou omita o campo." });
        

        _context.Fabricantes.Add(item);
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sql && (sql.Number == 2601 || sql.Number == 2627 || sql.Number == 547))
        {
            return Conflict(new { mensagem = "Não foi possível gravar: registro duplicado ou relacionamento inválido." });
        }
        return CreatedAtAction(nameof(GetPorId), new { id = item.FabricanteId }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, Fabricante item)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (id != item.FabricanteId) return BadRequest(new { mensagem = "O ID da URL deve ser igual ao ID do corpo." });
        var atual = await _context.Fabricantes.FindAsync(id);
        if (atual == null) return NotFound(new { mensagem = "Registro não encontrado." });
        

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
        var item = await _context.Fabricantes.FindAsync(id);
        if (item == null) return NotFound(new { mensagem = "Registro não encontrado." });
        if (await _context.Veiculos.AnyAsync(v => v.FabricanteId == id))
            return Conflict(new { mensagem = "O registro possui vínculos e não pode ser excluído." });
        _context.Fabricantes.Remove(item);
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
