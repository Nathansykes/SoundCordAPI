namespace Project.Domain.Exceptions;
public class DomainException : Exception
{
    public int StatusCode { get; set; } = 500;
    public DomainException(string message)
        : base(message) { }
    public DomainException(string message, Exception innerException)
        : base(message, innerException) { }

    public DomainException(string message, int statusCode)
        : base(message) => StatusCode = statusCode;
    public DomainException(string message, int statusCode, Exception innerException)
        : base(message, innerException) => StatusCode = statusCode;
}

public class ValidationException : DomainException
{
    public ValidationException(string message)
        : base(message, 400) { }
    public ValidationException(string message, Exception innerException)
        : base(message, 400, innerException) { }
}