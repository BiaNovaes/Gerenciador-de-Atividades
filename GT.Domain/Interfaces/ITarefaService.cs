using GT.Domain.Entites;

namespace GT.Domain.Interfaces;

public interface ITarefaService : IBaseService<Tarefa>
{
    Task<IEnumerable<Tarefa>> GetTarefasByUsuarioIdAsync(int usuarioId);
}
