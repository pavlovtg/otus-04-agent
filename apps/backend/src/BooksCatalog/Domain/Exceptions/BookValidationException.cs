namespace BooksCatalog.Domain.Exceptions;

internal class BookValidationException : Exception
{
    public BookValidationException(string message)
        : base(message)
    {
    }
}
