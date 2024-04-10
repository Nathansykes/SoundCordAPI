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

    public DomainException(string message, string description)
        : base(message) => Description = description;
    public DomainException(string message, Exception innerException, string description)
        : base(message, innerException) => Description = description;

    public DomainException(string message, int statusCode, string description)
        : base(message)
    {
        StatusCode = statusCode;
        Description = description;
    }

    public DomainException(string message, int statusCode, Exception innerException, string description)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        Description = description;
    }

    public string Description { get; set; } = "";
}

public class FriendlyException : DomainException
{
    public FriendlyException(string message, int statusCode) : base(message, statusCode)
    {
        if (statusCode >= 400)
            throw new InvalidOperationException("Friendly Exception Status Code should be < 400");
    }
    public FriendlyException(string message, int statusCode, string description) : base(message, statusCode, description)
    {
        if (statusCode >= 400)
            throw new InvalidOperationException("Friendly Exception Status Code should be < 400");
    }
}

public class ValidationException : DomainException
{
    public ValidationException(string message)
        : base(message, 400) { }
    public ValidationException(string message, Exception innerException)
        : base(message, 400, innerException) { }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string message)
        : base(message, 404) { }
    public NotFoundException(string message, Exception innerException)
        : base(message, 404, innerException) { }
}