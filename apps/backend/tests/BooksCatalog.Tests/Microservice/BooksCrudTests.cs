using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BooksCatalog.Tests.Microservice;

public class BooksCrudTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public BooksCrudTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task FullCrud_CreateReadUpdateDelete_Succeeds()
    {
        // Create
        var createRequest = new
        {
            title = "Новая книга",
            authors = new[] { "Иванов И.И." },
            isbn = "978-5-00-000001-1",
            publisher = "Тест",
            year = 2023,
            country = "Россия"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/v1/books", createRequest);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<BookResponseDto>(JsonOptions);
        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal("Новая книга", created.Title);

        // Read
        var getResponse = await _client.GetAsync($"/api/v1/books/{created.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetched = await getResponse.Content.ReadFromJsonAsync<BookResponseDto>(JsonOptions);
        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched.Id);
        Assert.Equal("Новая книга", fetched.Title);

        // Read all — contains created book
        var getAllResponse = await _client.GetAsync("/api/v1/books");
        Assert.Equal(HttpStatusCode.OK, getAllResponse.StatusCode);

        var all = await getAllResponse.Content.ReadFromJsonAsync<List<BookResponseDto>>(JsonOptions);
        Assert.NotNull(all);
        Assert.Contains(all, b => b.Id == created.Id);

        // Update
        var updateRequest = new
        {
            title = "Обновлённая книга",
            authors = new[] { "Петров П.П." },
            isbn = "978-5-00-000001-2",
            publisher = "Тест 2",
            year = 2024,
            country = "Россия"
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/v1/books/{created.Id}", updateRequest);
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updated = await updateResponse.Content.ReadFromJsonAsync<BookResponseDto>(JsonOptions);
        Assert.NotNull(updated);
        Assert.Equal("Обновлённая книга", updated.Title);
        Assert.Equal("Петров П.П.", updated.Authors[0]);

        // Delete
        var deleteResponse = await _client.DeleteAsync($"/api/v1/books/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Verify deleted
        var getAfterDelete = await _client.GetAsync($"/api/v1/books/{created.Id}");
        Assert.Equal(HttpStatusCode.BadRequest, getAfterDelete.StatusCode);
    }

    [Fact]
    public async Task SwaggerUi_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/swagger/index.html");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private record BookResponseDto(
        Guid Id,
        string Title,
        List<string> Authors,
        string? Isbn,
        string? Publisher,
        int? Year,
        string? Country);
}
