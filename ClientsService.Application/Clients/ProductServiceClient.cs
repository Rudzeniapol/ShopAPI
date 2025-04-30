using System.Net.Http.Headers;
using System.Net.Http.Json;
using ClientsService.Application.Exceptions;
using Microsoft.AspNetCore.Http;

namespace ClientsService.Application.Clients;

public class ProductServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductServiceClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token.ToString().Replace("Bearer ", ""));
        var response = await _httpClient.DeleteAsync("api/products/Product/userProducts", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new NoResponseFromServiceException("Product service was not successfully deactivated");
        }
    }

    public async Task ReturnAsync(CancellationToken cancellationToken = default)
    {
        var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token.ToString().Replace("Bearer ", ""));
        var response = await _httpClient.PostAsJsonAsync("api/products/Product/activation", "", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new NoResponseFromServiceException("Product service was not successfully activated");
        }
    }
}