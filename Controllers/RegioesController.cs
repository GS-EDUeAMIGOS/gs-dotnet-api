using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViaCidada.Ouvidoria.Api.Data;
using ViaCidada.Ouvidoria.Api.Models;

namespace ViaCidada.Ouvidoria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegioesController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegiaoUrbana>>> GetAll(CancellationToken cancellationToken)
    {
        var regioes = await context.RegioesUrbanas.AsNoTracking().ToListAsync(cancellationToken);
        return Ok(regioes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RegiaoUrbana>> GetById(int id, CancellationToken cancellationToken)
    {
        var regiao = await context.RegioesUrbanas.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        return regiao is null
            ? NotFound(new { mensagem = $"Região urbana com ID {id} não encontrada." })
            : Ok(regiao);
    }

    [HttpPost]
    public async Task<ActionResult<RegiaoUrbana>> Create(RegiaoUrbana regiao, CancellationToken cancellationToken)
    {
        context.RegioesUrbanas.Add(regiao);
        await context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = regiao.Id }, regiao);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, RegiaoUrbana regiao, CancellationToken cancellationToken)
    {
        var existente = await context.RegioesUrbanas.FindAsync([id], cancellationToken);
        if (existente is null)
            return NotFound(new { mensagem = $"Região urbana com ID {id} não encontrada." });

        existente.Nome = regiao.Nome;
        existente.ZonaCidade = regiao.ZonaCidade;
        existente.Subprefeitura = regiao.Subprefeitura;
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var regiao = await context.RegioesUrbanas.FindAsync([id], cancellationToken);
        if (regiao is null)
            return NotFound(new { mensagem = $"Região urbana com ID {id} não encontrada." });

        context.RegioesUrbanas.Remove(regiao);
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
