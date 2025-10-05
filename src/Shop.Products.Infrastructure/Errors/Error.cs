using FluentResults;

namespace Shop.Products.Infrastructure.Errors;

/// <summary>
/// Represents an error indicating that a specified resource was not found.
/// </summary>
public sealed record NotFoundError : Error
{
    public NotFoundError(string message)
    {
        Message = message;
    }
}

/// <summary>
/// Provides a base class for representing errors.
/// </summary>
public abstract record Error : IError
{
    public string Message { get; init; } = string.Empty;
    public Dictionary<string, object> Metadata { get; init; } = [];
    public List<IError> Reasons { get; init; } = [];
}