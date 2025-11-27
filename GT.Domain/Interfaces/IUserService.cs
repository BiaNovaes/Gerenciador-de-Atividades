using GT.Domain.Entites;

namespace GT.Domain.Interfaces;

public interface IUserService : IBaseService<User>
{
    Task<User> AuthenticateAsync(string username, string password);
}
