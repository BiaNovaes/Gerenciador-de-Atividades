using GT.Infra.enum;
using Microsoft.AspNetCore.Mvc;
using GT.Domain.Entites;
using GT.Domain.Interfaces;
using FluentValidation; 

namespace GT.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : GTBaseController<Tarefa>
    {
        public TarefaController(ITarefaService service, IValidator<Tarefa> validator) 
            : base(service, validator)
        {
        }

        [HttpPut("{id}/avancar-status")]
        public async Task<IActionResult> AvancarStatus(int id)
        {
            var tarefa = await _service.GetByIdAsync(id);
            if (tarefa == null) return NotFound("Tarefa não encontrada.");

            if (tarefa.Status == TarefaEnum.Pendente)
            {
                tarefa.Status = TarefaEnum.EmAndamento;
                await _service.UpdateAsync(tarefa);
                return Ok(new { mensagem = "Tarefa iniciada!", status = tarefa.Status });
            }
            else if (tarefa.Status == TarefaEnum.EmAndamento)
            {
                tarefa.Status = TarefaEnum.Concluida;
                await _service.UpdateAsync(tarefa);
                return Ok(new { mensagem = "Tarefa concluída!", status = tarefa.Status });
            }
            
            return BadRequest("A tarefa já está concluída.");
        }
    }
}