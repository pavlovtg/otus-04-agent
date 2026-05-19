using BooksCatalog.Application.Dto;
using BooksCatalog.Domain;
using BooksCatalog.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace BooksCatalog.Application.Services;

internal class BookService
{
    private readonly IBookRepository _repository;
    private readonly ILogger<BookService> _logger;

    public BookService(IBookRepository repository, ILogger<BookService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async IAsyncEnumerable<BookDto> GetAllAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var book in _repository.GetAllAsync(cancellationToken))
        {
            yield return MapToDto(book);
        }
    }

    public async Task<BookDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var book = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new BookNotFoundException(id);

        return MapToDto(book);
    }

    public async Task<BookDto> CreateAsync(CreateBookDto dto, CancellationToken cancellationToken)
    {
        var book = Book.Create(
            dto.Title,
            dto.Authors?.ToList(),
            dto.Isbn,
            dto.Publisher,
            dto.Year,
            dto.Country);

        await _repository.AddAsync(book, cancellationToken);

        _logger.LogInformation("Book created {BookId} {BookTitle}", book.Id, book.Title);

        return MapToDto(book);
    }

    public async Task<BookDto> UpdateAsync(Guid id, UpdateBookDto dto, CancellationToken cancellationToken)
    {
        var book = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new BookNotFoundException(id);

        book.Update(
            dto.Title,
            dto.Authors?.ToList(),
            dto.Isbn,
            dto.Publisher,
            dto.Year,
            dto.Country);

        await _repository.UpdateAsync(book, cancellationToken);

        _logger.LogInformation("Book updated {BookId}", id);

        return MapToDto(book);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var book = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new BookNotFoundException(id);

        await _repository.DeleteAsync(book.Id, cancellationToken);

        _logger.LogInformation("Book deleted {BookId}", id);
    }

    private static BookDto MapToDto(Book book)
    {
        return new BookDto(
            book.Id,
            book.Title,
            book.Authors,
            book.Isbn,
            book.Publisher,
            book.Year,
            book.Country);
    }
}
