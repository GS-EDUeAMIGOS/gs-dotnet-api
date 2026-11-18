using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViaCidada.Ouvidoria.Api.Data;
using ViaCidada.Ouvidoria.Api.Models;

namespace ViaCidada.Ouvidoria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropostasController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropostaMelhoria>>> GetAll(CancellationToken cancellationToken)
    {
        var propostas = await context.PropostasMelhoria.AsNoTracking().ToListAsync(cancellationToken);
        return Ok(propostas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PropostaMelhoria>> GetById(int id, CancellationToken cancellationToken)
    {
        var proposta = await context.PropostasMelhoria.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        return proposta is null
            ? NotFound(new { mensagem = $"Proposta de melhoria com ID {id} não encontrada." })
            : Ok(proposta);
    }

    [HttpPost]
    public async Task<ActionResult<PropostaMelhoria>> Create(PropostaMelhoria proposta, CancellationToken cancellationToken)
    {
        var regiaoExiste = await context.RegioesUrbanas
            .AnyAsync(regiao => regiao.Id == proposta.RegiaoUrbanaId, cancellationToken);
        if (!regiaoExiste)
            return BadRequest(new { mensagem = $"A região urbana com ID {proposta.RegiaoUrbanaId} não existe." });

        proposta.DataCriacao = DateTime.UtcNow;
        proposta.RegiaoUrbana = null;
        context.PropostasMelhoria.Add(proposta);
        await context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = proposta.Id }, proposta);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PropostaMelhoria proposta, CancellationToken cancellationToken)
    {
        var existente = await context.PropostasMelhoria.FindAsync([id], cancellationToken);
        if (existente is null)
            return NotFound(new { mensagem = $"Proposta de melhoria com ID {id} não encontrada." });

        var regiaoExiste = await context.RegioesUrbanas
            .AnyAsync(regiao => regiao.Id == proposta.RegiaoUrbanaId, cancellationToken);
        if (!regiaoExiste)
            return BadRequest(new { mensagem = $"A região urbana com ID {proposta.RegiaoUrbanaId} não existe." });

        existente.Titulo = proposta.Titulo;
        existente.Descricao = proposta.Descricao;
        existente.Categoria = proposta.Categoria;
        existente.CodigoLinhaOuParada = proposta.CodigoLinhaOuParada;
        existente.Status = proposta.Status;
        existente.RegiaoUrbanaId = proposta.RegiaoUrbanaId;
        existente.RegiaoUrbana = null;
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var proposta = await context.PropostasMelhoria.FindAsync([id], cancellationToken);
        if (proposta is null)
            return NotFound(new { mensagem = $"Proposta de melhoria com ID {id} não encontrada." });

        context.PropostasMelhoria.Remove(proposta);
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
