using BooksCatalog.Domain.Exceptions;

namespace BooksCatalog.Domain;

internal class Book
{
    private Book()
    {
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public List<string> Authors { get; private set; } = [];

    public string? Isbn { get; private set; }

    public string? Publisher { get; private set; }

    public int? Year { get; private set; }

    public string? Country { get; private set; }

    public static Book Create(
        string title,
        List<string>? authors = null,
        string? isbn = null,
        string? publisher = null,
        int? year = null,
        string? country = null)
    {
        ValidateTitle(title);
        ValidateYear(year);

        return new Book
        {
            Id = Guid.NewGuid(),
            Title = title,
            Authors = authors ?? [],
            Isbn = isbn,
            Publisher = publisher,
            Year = year,
            Country = country
        };
    }

    public void Update(
        string title,
        List<string>? authors,
        string? isbn,
        string? publisher,
        int? year,
        string? country)
    {
        ValidateTitle(title);
        ValidateYear(year);

        Title = title;
        Authors = authors ?? [];
        Isbn = isbn;
        Publisher = publisher;
        Year = year;
        Country = country;
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new BookValidationException("Title is required.");
        }
    }

    private static void ValidateYear(int? year)
    {
        if (year is null)
        {
            return;
        }

        int currentYear = DateTime.UtcNow.Year;

        if (year < 0 || year > currentYear)
        {
            throw new BookValidationException($"Year must be between 0 and {currentYear}.");
        }
    }
}
