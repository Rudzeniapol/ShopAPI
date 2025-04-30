using System.Data;
using System.Net;
using Azure.Core;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using ProductService.Application.Exceptions;

namespace ProductsService.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    private async void ConfigureResponse(HttpContext httpContext, int StatusCode, object? Json, string logMessage)
    {
        _logger.LogError(logMessage);
        httpContext.Response.StatusCode = StatusCode;
        await httpContext.Response.WriteAsJsonAsync(Json);
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadRequestException ex)
        {
            ConfigureResponse(context,
                (int)HttpStatusCode.BadRequest,
                new { error = "Invalid request", code = 400, details = ex.Message },
                ex.Message);
        }
        catch (NotFoundException ex)
        {
            ConfigureResponse(context,
                (int)HttpStatusCode.NotFound,
                new { error = "Entity not found", code = 404, details = ex.Message },
                ex.Message);
        }
        catch (EntityExistsException ex)
        {
            ConfigureResponse(context,
                (int)HttpStatusCode.Conflict,
                new { error = "Entity already exists", code = 409, details = ex.Message },
                ex.Message);
        }
        catch (SecurityTokenException ex)
        {
            ConfigureResponse(context,
                (int)HttpStatusCode.Unauthorized,
                new { error = "Token error", code = 401, details = ex.Message },
                ex.Message);
        }
        catch (TaskCanceledException ex)
        {
            ConfigureResponse(context,
                (int)HttpStatusCode.RequestTimeout,
                new { error = "Request timed out", code = 408, details = ex.Message },
                "Request timed out");
        }
        catch (OperationCanceledException ex)
        {
            ConfigureResponse(context,
                499,
                new { error = "Request canceled", code = 499, details = ex.Message },
                "Request canceled");
        }
        catch (UnauthorizedAccessException ex)
        {
            ConfigureResponse(context,
                (int)HttpStatusCode.Unauthorized,
                new { error = "Authorization error", code = 401, details = ex.Message },
                ex.Message);
        }
        catch (ValidationException ex)
        {
            ConfigureResponse(context,
                (int)HttpStatusCode.BadRequest,
                new { error = "Validation error", code = 400, details = ex.Message },
                ex.Message);
        }
        catch (DataException ex)
        {
            _logger.LogError(ex.Message);
        }
        catch (Exception ex)
        {
            ConfigureResponse(context,
                (int)HttpStatusCode.InternalServerError,
                new { error = $"An error occurred: {ex.Message}", code = ex.HResult, details = ex.Message },
                "Internal server error");
        }
    }
}