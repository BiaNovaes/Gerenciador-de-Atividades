using GT.Domain.Entites;
using GT.Domain.Interfaces;
using BCrypt.Net;                     

namespace GT.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _repository;

        public UserService(IBaseRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<User> AddAsync(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password, workFactor: 12);
            
            return await _repository.AddAsync(entity);
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var users = await _repository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user == null)
                return null;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!isPasswordValid)
                return null;

            return user;
        }
        
        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
        public async Task<IEnumerable<User>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<User> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        
        public async Task<User> UpdateAsync(User entity)
        {
             return await _repository.UpdateAsync(entity);
        }
    }
}