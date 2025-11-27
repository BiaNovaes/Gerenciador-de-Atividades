using GT.Infra.enum;

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

        public Tarefa() { }

        public Tarefa(string titulo, string descricao, TarefaEnum status, int usuarioId)
        {
            Titulo = titulo;
            Descricao = descricao;
            Status = status;
            UsuarioId = usuarioId;
            DataCriacao = DateTime.UtcNow;
        }
    }
}