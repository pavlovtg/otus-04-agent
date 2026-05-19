namespace BooksCatalog.Domain.Exceptions;

internal class BookNotFoundException : Exception
{
    public BookNotFoundException(Guid id)
        : base($"Book with id '{id}' was not found.")
    {
    }
}
