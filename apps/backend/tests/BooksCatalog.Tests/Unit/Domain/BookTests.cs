using BooksCatalog.Domain;
using BooksCatalog.Domain.Exceptions;

namespace BooksCatalog.Tests.Unit.Domain;

public class BookTests
{
    [Fact]
    public void Create_WithValidData_ReturnsBook()
    {
        var book = Book.Create("Война и мир", new List<string> { "Толстой" }, "isbn", "АСТ", 1869, "Россия");

        Assert.NotEqual(Guid.Empty, book.Id);
        Assert.Equal("Война и мир", book.Title);
        Assert.Equal(new List<string> { "Толстой" }, book.Authors);
        Assert.Equal("isbn", book.Isbn);
        Assert.Equal("АСТ", book.Publisher);
        Assert.Equal(1869, book.Year);
        Assert.Equal("Россия", book.Country);
    }

    [Fact]
    public void Create_WithEmptyTitle_ThrowsBookValidationException()
    {
        Assert.Throws<BookValidationException>(() => Book.Create(""));
    }

    [Fact]
    public void Create_WithWhitespaceTitle_ThrowsBookValidationException()
    {
        Assert.Throws<BookValidationException>(() => Book.Create("   "));
    }

    [Fact]
    public void Create_WithNullAuthors_SetsEmptyList()
    {
        var book = Book.Create("Заголовок");

        Assert.NotNull(book.Authors);
        Assert.Empty(book.Authors);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3000)]
    public void Create_WithInvalidYear_ThrowsBookValidationException(int year)
    {
        Assert.Throws<BookValidationException>(() => Book.Create("Заголовок", year: year));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1000)]
    [InlineData(2024)]
    public void Create_WithValidYear_DoesNotThrow(int year)
    {
        var exception = Record.Exception(() => Book.Create("Заголовок", year: year));
        Assert.Null(exception);
    }

    [Fact]
    public void Update_WithValidData_UpdatesBook()
    {
        var book = Book.Create("Старый заголовок");

        book.Update("Новый заголовок", new List<string> { "Автор" }, "isbn2", "Эксмо", 2000, "Россия");

        Assert.Equal("Новый заголовок", book.Title);
        Assert.Equal(new List<string> { "Автор" }, book.Authors);
        Assert.Equal("isbn2", book.Isbn);
        Assert.Equal("Эксмо", book.Publisher);
        Assert.Equal(2000, book.Year);
        Assert.Equal("Россия", book.Country);
    }

    [Fact]
    public void Update_WithEmptyTitle_ThrowsBookValidationException()
    {
        var book = Book.Create("Заголовок");

        Assert.Throws<BookValidationException>(() => book.Update("", null, null, null, null, null));
    }

    [Fact]
    public void Update_WithInvalidYear_ThrowsBookValidationException()
    {
        var book = Book.Create("Заголовок");

        Assert.Throws<BookValidationException>(() => book.Update("Заголовок", null, null, null, -1, null));
    }
}
