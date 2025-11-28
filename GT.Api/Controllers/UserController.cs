    using GT.Application.DTOs; 
    using GT.Domain.Entites;
    using GT.Domain.Interfaces;
    using FluentValidation; 
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Text;
    using System.Security.Claims;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.IdentityModel.Tokens;

    namespace GT.Api.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class UserController : GTBaseController<User>
        {
            private readonly IConfiguration _configuration;

            public UserController(IUserService service, IValidator<User> validator, IConfiguration configuration) 
                : base(service, validator)
            {
                _configuration = configuration;
            }

            [AllowAnonymous]
            [HttpPost("authenticate")]
            public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
            {
                var service = (IUserService)_service;
                var user = await service.AuthenticateAsync(request.Username, request.Password);
                
                if (user == null)
                    return Unauthorized(new { message = "Usuário ou senha inválidos." });

             
                var token = GerarToken(user);

                
                return Ok(new 
                { 
                    Id = user.Id, 
                    Username = user.Username, 
                    Token = token 
                });
            }

            [HttpDelete("{id}")]
            public override Task<IActionResult> Delete(int id)
            {
                return Task.FromResult<IActionResult>(StatusCode(405, "Não é permitido excluir usuários diretamente."));
            }

            private string GerarToken(User user)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(GT.Api.Settings.Secret);
                

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        }
    }