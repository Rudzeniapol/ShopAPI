namespace ClientsService.Application.Exceptions;

public class NoResponseFromServiceException : Exception
{
    public NoResponseFromServiceException() : base("No response from the service.") { }
    public NoResponseFromServiceException(string message) : base(message) { }
    public NoResponseFromServiceException(string message, Exception innerException) : base(message, innerException) { }
}