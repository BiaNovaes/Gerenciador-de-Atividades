using Microsoft.AspNetCore.Mvc;
using GT.Domain.Interfaces;
using FluentValidation; 
using Microsoft.AspNetCore.Authorization;
using FluentValidation.Results;

namespace GT.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GTBaseController<T> : ControllerBase where T : class
    {
        protected readonly IBaseService<T> _service;
        protected readonly IValidator<T> _validator; 

        
        public GTBaseController(IBaseService<T> service, IValidator<T> validator)
        {
            _service = service;
            _validator = validator;
        }

                
        protected async Task<ValidationResult> ValidateAsync(T entity)
        {
            return await _validator.ValidateAsync(entity);
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound("Registro não encontrado.");
            return Ok(item);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] T entity)
        {            
            var validationResult = await ValidateAsync(entity);
            
            if (!validationResult.IsValid)
            {
                
                return BadRequest(validationResult.Errors.Select(e => new { 
                    Campo = e.PropertyName, 
                    Erro = e.ErrorMessage 
                }));
            }

            var createdItem = await _service.AddAsync(entity);
            return Ok(createdItem);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(int id, [FromBody] T entity)
        {
            
            var validationResult = await ValidateAsync(entity);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new { 
                    Campo = e.PropertyName, 
                    Erro = e.ErrorMessage 
                }));
            }

            try 
            {
                await _service.UpdateAsync(entity);
                return Ok(entity);
            }
            catch
            {
                return BadRequest("Erro ao atualizar. Verifique os dados.");
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            
            return NoContent(); 
        }
    }
}