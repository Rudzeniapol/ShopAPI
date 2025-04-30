namespace ClientsService.Application.Clients.Interfaces;

public interface IProductServiceClient
{
    public Task DeleteAsync(CancellationToken cancellationToken = default);
    public Task ReturnAsync(CancellationToken cancellationToken = default);
}