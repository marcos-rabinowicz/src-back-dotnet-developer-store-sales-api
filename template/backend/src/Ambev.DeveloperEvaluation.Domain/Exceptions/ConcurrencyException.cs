namespace Ambev.DeveloperEvaluation.Domain.Exceptions;

public sealed class ConcurrencyException : Exception
{
    public ConcurrencyException(string message, Exception? inner = null) : base(message, inner) { }
}