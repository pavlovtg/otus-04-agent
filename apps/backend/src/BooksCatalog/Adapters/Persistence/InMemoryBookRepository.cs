using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using BooksCatalog.Domain;

namespace BooksCatalog.Adapters.Persistence;

internal class InMemoryBookRepository : IBookRepository
{
    private readonly ConcurrentDictionary<Guid, Book> _store = new();

    public async IAsyncEnumerable<Book> GetAllAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var book in _store.Values)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return book;
        }

        await Task.CompletedTask;
    }

    public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _store.TryGetValue(id, out var book);
        return Task.FromResult(book);
    }

    public Task AddAsync(Book book, CancellationToken cancellationToken)
    {
        _store[book.Id] = book;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Book book, CancellationToken cancellationToken)
    {
        _store[book.Id] = book;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        _store.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}
