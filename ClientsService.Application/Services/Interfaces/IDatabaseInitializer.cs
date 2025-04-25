namespace ClientsService.Application.Services.Interfaces;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
}