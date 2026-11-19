using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViaCidada.Ouvidoria.Api.Data;
using ViaCidada.Ouvidoria.Api.Models;

namespace ViaCidada.Ouvidoria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropostasController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropostaMelhoria>>> GetAll(CancellationToken cancellationToken)
    {
        var propostas = await db.PropostasMelhoria
            .AsNoTracking()
            .Include(p => p.RegiaoUrbana)
            .OrderByDescending(p => p.Id)
            .ToListAsync(cancellationToken);

        return Ok(propostas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PropostaMelhoria>> GetById(int id, CancellationToken cancellationToken)
    {
        var proposta = await db.PropostasMelhoria
            .AsNoTracking()
            .Include(p => p.RegiaoUrbana)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (proposta is null)
        {
            return NotFound(new { mensagem = $"Proposta com ID {id} não encontrada." });
        }

        return Ok(proposta);
    }

    [HttpPost]
    public async Task<ActionResult<PropostaMelhoria>> Create([FromBody] PropostaMelhoria proposta, CancellationToken cancellationToken)
    {
        var regiao = await db.RegioesUrbanas.FindAsync(new object[] { proposta.RegiaoUrbanaId }, cancellationToken);
        if (regiao is null)
        {
            return BadRequest(new { mensagem = $"A região urbana com ID {proposta.RegiaoUrbanaId} não existe." });
        }

        proposta.Id = 0;
        if (string.IsNullOrWhiteSpace(proposta.Status))
        {
            proposta.Status = "RECEBIDA";
        }

        db.PropostasMelhoria.Add(proposta);
        await db.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = proposta.Id }, proposta);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PropostaMelhoria proposta, CancellationToken cancellationToken)
    {
        var existente = await db.PropostasMelhoria.FindAsync(new object[] { id }, cancellationToken);
        if (existente is null)
        {
            return NotFound(new { mensagem = $"Proposta com ID {id} não encontrada." });
        }

        var regiao = await db.RegioesUrbanas.FindAsync(new object[] { proposta.RegiaoUrbanaId }, cancellationToken);
        if (regiao is null)
        {
            return BadRequest(new { mensagem = $"A região urbana com ID {proposta.RegiaoUrbanaId} não existe." });
        }

        existente.Titulo = proposta.Titulo;
        existente.Descricao = proposta.Descricao;
        existente.Categoria = proposta.Categoria;
        existente.CodigoLinhaOuParada = proposta.CodigoLinhaOuParada;
        existente.Status = string.IsNullOrWhiteSpace(proposta.Status) ? existente.Status : proposta.Status;
        existente.RegiaoUrbanaId = proposta.RegiaoUrbanaId;

        await db.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var proposta = await db.PropostasMelhoria.FindAsync(new object[] { id }, cancellationToken);
        if (proposta is null)
        {
            return NotFound(new { mensagem = $"Proposta com ID {id} não encontrada." });
        }

        db.PropostasMelhoria.Remove(proposta);
        await db.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
