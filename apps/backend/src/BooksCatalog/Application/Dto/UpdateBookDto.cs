namespace BooksCatalog.Application.Dto;

internal record UpdateBookDto(
    string Title,
    IReadOnlyList<string>? Authors,
    string? Isbn,
    string? Publisher,
    int? Year,
    string? Country);
