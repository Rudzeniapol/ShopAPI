using ClientsService.Application.DTOs;
using ClientsService.Domain.Interfaces;
using ClientsService.Domain.Models;
using ClientsService.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ClientsService.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ClientsDbContext context) : base(context)
    {
    }
    
    public async Task<User?> GetUserByUserNameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username.Equals(username), cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email.Equals(email), cancellationToken);
    }
}