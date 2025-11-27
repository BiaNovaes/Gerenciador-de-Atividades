using GT.Domain.Entites;
using GT.Domain.Interfaces;
using GT.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace GT.Infra.Repository
{
    public class TarefaRepository : BaseRepository<Tarefa>, IBaseRepository<Tarefa>
    {
        public TarefaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Tarefa>> GetAllAsync()
        {
            return await _context.Tarefas
                .Include(t => t.Usuario)
                .ToListAsync();
        }

        public override async Task<Tarefa> GetByIdAsync(int id)
        {
            return await _context.Tarefas
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
