using GT.Domain.Enums;

namespace GT.Domain.Entites
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public TarefaEnum Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public int UsuarioId { get; set; }
        public User Usuario { get; set; }
    }
}