using ClientsService.Application.DTOs;
using ClientsService.Domain.Interfaces;
using ClientsService.Domain.Models;

namespace ClientsService.Persistence.Repositories;
//Implement this with DB context
public class UserRepository //: IUserRepository
{
    public Task AddAsync(User user, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await Task.FromResult((User)null);
    }
    
    
}