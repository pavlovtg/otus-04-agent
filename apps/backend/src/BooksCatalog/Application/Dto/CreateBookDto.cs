namespace BooksCatalog.Application.Dto;

internal record CreateBookDto(
    string Title,
    IReadOnlyList<string>? Authors,
    string? Isbn,
    string? Publisher,
    int? Year,
    string? Country);
