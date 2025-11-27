using GT.Domain.Enums;

namespace GT.Application.DTOs;

public class TarefaDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public TarefaEnum Status { get; set; }
    public DateTime DataCriacao { get; set; }
    public int UsuarioId { get; set; }
}
