namespace BooksCatalog.Domain;

internal interface IBookRepository
{
    IAsyncEnumerable<Book> GetAllAsync(CancellationToken cancellationToken);

    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task AddAsync(Book book, CancellationToken cancellationToken);

    Task UpdateAsync(Book book, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
