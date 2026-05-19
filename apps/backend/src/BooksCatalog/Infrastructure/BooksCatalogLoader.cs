using System.Text.Json;
using System.Text.Json.Serialization;
using BooksCatalog.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BooksCatalog.Infrastructure;

internal class BooksCatalogLoader : IHostedService
{
    private readonly IBookRepository _repository;
    private readonly BooksCatalogLoaderOptions _options;
    private readonly ILogger<BooksCatalogLoader> _logger;

    public BooksCatalogLoader(
        IBookRepository repository,
        IOptions<BooksCatalogLoaderOptions> options,
        ILogger<BooksCatalogLoader> logger)
    {
        _repository = repository;
        _options = options.Value;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_options.BooksFilePath))
        {
            _logger.LogWarning("Books file not found: {Path}", _options.BooksFilePath);
            return;
        }

        await using var stream = File.OpenRead(_options.BooksFilePath);

        var records = await JsonSerializer.DeserializeAsync<List<BookRecord>>(
            stream,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
            cancellationToken);

        if (records is null)
        {
            return;
        }

        foreach (var record in records)
        {
            var book = Book.Create(
                record.Title,
                record.Authors,
                record.Isbn,
                record.Publisher,
                record.Year,
                record.Country);

            await _repository.AddAsync(book, cancellationToken);
        }

        _logger.LogInformation("Loaded {Count} books from {Path}", records.Count, _options.BooksFilePath);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private record BookRecord(
        string Title,
        [property: JsonPropertyName("authors")] List<string>? Authors,
        string? Isbn,
        string? Publisher,
        int? Year,
        string? Country);
}
