namespace BooksCatalog.Application.Dto;

internal record BookDto(
    Guid Id,
    string Title,
    IReadOnlyList<string> Authors,
    string? Isbn,
    string? Publisher,
    int? Year,
    string? Country);
