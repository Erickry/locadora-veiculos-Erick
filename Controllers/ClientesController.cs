using LocadoraVeiculosErick.Data;
using LocadoraVeiculosErick.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculosErick.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ApplicationContext _context;
    public ClientesController(ApplicationContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetTodos() =>
        Ok(await _context.Clientes.AsNoTracking().OrderBy(x => x.ClienteId).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cliente>> GetPorId(int id)
    {
        var item = await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.ClienteId == id);
        if (item == null) return NotFound(new { mensagem = "Registro não encontrado." });
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> Post(Cliente item)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (item.ClienteId != 0) return BadRequest(new { mensagem = "O ID é gerado pelo banco; envie zero ou omita o campo." });
        item.CPF = item.CPF.Replace(".", "").Replace("-", ""); item.Email = item.Email.Trim().ToLowerInvariant();
        if (await _context.Clientes.AnyAsync(c => c.ClienteId != item.ClienteId && (c.CPF == item.CPF || c.Email == item.Email)))
            return Conflict(new { mensagem = "CPF ou e-mail já cadastrado." });
        _context.Clientes.Add(item);
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sql && (sql.Number == 2601 || sql.Number == 2627 || sql.Number == 547))
        {
            return Conflict(new { mensagem = "Não foi possível gravar: registro duplicado ou relacionamento inválido." });
        }
        return CreatedAtAction(nameof(GetPorId), new { id = item.ClienteId }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, Cliente item)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (id != item.ClienteId) return BadRequest(new { mensagem = "O ID da URL deve ser igual ao ID do corpo." });
        var atual = await _context.Clientes.FindAsync(id);
        if (atual == null) return NotFound(new { mensagem = "Registro não encontrado." });
        item.CPF = item.CPF.Replace(".", "").Replace("-", ""); item.Email = item.Email.Trim().ToLowerInvariant();
        if (await _context.Clientes.AnyAsync(c => c.ClienteId != item.ClienteId && (c.CPF == item.CPF || c.Email == item.Email)))
            return Conflict(new { mensagem = "CPF ou e-mail já cadastrado." });
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
        var item = await _context.Clientes.FindAsync(id);
        if (item == null) return NotFound(new { mensagem = "Registro não encontrado." });
        if (await _context.Alugueis.AnyAsync(a => a.ClienteId == id) ||
            await _context.Reservas.AnyAsync(r => r.ClienteId == id))
            return Conflict(new { mensagem = "O registro possui vínculos e não pode ser excluído." });
        _context.Clientes.Remove(item);
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

