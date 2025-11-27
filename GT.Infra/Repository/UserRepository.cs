using GT.Domain.Entites;
using GT.Domain.Interfaces;
using GT.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace GT.Infra.Repository
{
    public class UserRepository : BaseRepository<User>, IBaseRepository<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Tarefas)
                .ToListAsync();
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Tarefas)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
