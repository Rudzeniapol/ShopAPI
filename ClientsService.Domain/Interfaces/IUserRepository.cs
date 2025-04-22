using ClientsService.Domain.Models;

namespace ClientsService.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByUserNameAsync(string username, CancellationToken cancellationToken = default);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);   
}