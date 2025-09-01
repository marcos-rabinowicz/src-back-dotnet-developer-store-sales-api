namespace Ambev.DeveloperEvaluation.Domain.Exceptions;

public sealed class DuplicateKeyException : Exception
{
    public DuplicateKeyException(string message, Exception? inner = null) : base(message, inner) { }
}