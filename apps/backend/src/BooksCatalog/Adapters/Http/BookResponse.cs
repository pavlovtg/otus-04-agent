namespace BooksCatalog.Adapters.Http;

internal record BookResponse(
    Guid Id,
    string Title,
    IReadOnlyList<string> Authors,
    string? Isbn,
    string? Publisher,
    int? Year,
    string? Country);
