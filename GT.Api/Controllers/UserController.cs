using GT.Application.DTOs; 
using GT.Domain.Entites;
using GT.Domain.Interfaces;
using FluentValidation; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GT.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : GTBaseController<User>
    {
        public UserController(IUserService service, IValidator<User> validator) 
            : base(service, validator)
        {
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            var service = (IUserService)_service;
            var user = await service.AuthenticateAsync(request.Username, request.Password);
            
            if (user == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos." });

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public override Task<IActionResult> Delete(int id)
        {
            return Task.FromResult<IActionResult>(StatusCode(405, "Não é permitido excluir usuários diretamente."));
        }
    }
}