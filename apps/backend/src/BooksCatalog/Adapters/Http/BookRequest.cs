namespace BooksCatalog.Adapters.Http;

internal record CreateBookRequest(
    string Title,
    IReadOnlyList<string>? Authors,
    string? Isbn,
    string? Publisher,
    int? Year,
    string? Country);

internal record UpdateBookRequest(
    string Title,
    IReadOnlyList<string>? Authors,
    string? Isbn,
    string? Publisher,
    int? Year,
    string? Country);
