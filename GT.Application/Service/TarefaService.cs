using GT.Domain.Entites;
using GT.Domain.Interfaces;

namespace GT.Application.Service
{
    public class TarefaService : ITarefaService
    {
        private readonly IBaseRepository<Tarefa> _repository;

        public TarefaService(IBaseRepository<Tarefa> repository)
        {
            _repository = repository;
        }

        public async Task<Tarefa> AddAsync(Tarefa entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.DataCriacao = DateTime.UtcNow;
            return await _repository.AddAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Tarefa>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Tarefa> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Tarefa> UpdateAsync(Tarefa entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return await _repository.UpdateAsync(entity);
        }

        public async Task<IEnumerable<Tarefa>> GetTarefasByUsuarioIdAsync(int usuarioId)
        {
            var tarefas = await _repository.GetAllAsync();
            return tarefas.Where(t => t.UsuarioId == usuarioId);
        }
    }
}