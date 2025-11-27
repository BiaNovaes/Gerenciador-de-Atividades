using Microsoft.AspNetCore.Mvc;
using GT.Domain.Entites;
using GT.Domain.Interfaces;
 
namespace GT.Api.Controllers
{
    [ApiController]
    [Route("api/vulneravel")]
    public class VulneravelController : ControllerBase
    {
        private readonly IBaseRepository<User> _userRepository;
 
        public VulneravelController(IBaseRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
 
        // ERRO 1: Vazamento de Dados Sensíveis (Sensitive Data Exposure)
        // Esse endpoint retorna TUDO do usuário, incluindo a senha criptografada.
        [HttpGet("usuarios-expostos")]
        public async Task<IActionResult> GetUsuariosComSenha()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }
 
        // ERRO 2: Autenticação Quebrada (Broken Authentication)
        // se o usuário digitar "admin" no nome, ele entra com QUALQUER senha.
        [HttpPost("login-inseguro")]
        public async Task<IActionResult> LoginInseguro(string username, string password)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == username);
 
            if (user == null) return NotFound("Usuário não existe");
 
            if (username == "admin" || username == "hackeme")
            {
                // Deixa passar sem checar a senha!
                return Ok(new { mensagem = "Logado via Backdoor! Cuidado!", usuario = user });
            }
 
            if (user.Password == password)
            {
                return Ok(new { mensagem = "Logado!", usuario = user });
            }
 
            return Unauthorized("Senha incorreta (mas tente 'admin'...)");
        }
 
        // ERRO 3: SQL Injection (Simulação) ou IDOR
        // Aqui permitimos deletar um usuário apenas passando o ID na URL,
        // sem verificar se quem está pedindo tem permissão de administrador.
        [HttpDelete("deletar-sem-permissao/{id}")]
        public async Task<IActionResult> DeletarQualquerUm(int id)
        {
            var deleted = await _userRepository.DeleteAsync(id);
           
            if (deleted) return Ok($"Usuário {id} deletado com sucesso (sem segurança nenhuma).");
           
            return NotFound();
        }
    }
}