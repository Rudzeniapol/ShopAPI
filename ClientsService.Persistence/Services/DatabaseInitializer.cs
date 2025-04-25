using ClientsService.Application.Services.Interfaces;
using ClientsService.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ClientsService.Persistence.Services;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly ClientsDbContext _context;

    public DatabaseInitializer(ClientsDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.MigrateAsync();
    }
}