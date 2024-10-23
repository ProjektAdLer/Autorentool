using System.Net;

namespace BusinessLogic.ErrorManagement.BackendAccess;

public class BackendHttpRequestException : HttpRequestException
{
    
    public string? ErrorType { get; set; }
    
    public BackendHttpRequestException(string? message, Exception? inner, HttpStatusCode? statusCode, string? type)
        : base(message, inner, statusCode)
    {
        ErrorType = type;
    }
}