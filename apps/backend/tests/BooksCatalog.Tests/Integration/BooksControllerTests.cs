using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BooksCatalog.Tests.Integration;

public class BooksControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BooksControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/v1/books");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_NotFound_ReturnsBadRequest()
    {
        var response = await _client.GetAsync($"/api/v1/books/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidBody_ReturnsCreated()
    {
        var request = new
        {
            title = "Тестовая книга",
            authors = new[] { "Автор" },
            isbn = "978-5-00-000000-1",
            publisher = "Издатель",
            year = 2020,
            country = "Россия"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/books", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithEmptyTitle_ReturnsBadRequest()
    {
        var request = new { title = "" };

        var response = await _client.PostAsJsonAsync("/api/v1/books", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_NotFound_ReturnsBadRequest()
    {
        var request = new { title = "Книга" };

        var response = await _client.PutAsJsonAsync($"/api/v1/books/{Guid.NewGuid()}", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_NotFound_ReturnsBadRequest()
    {
        var response = await _client.DeleteAsync($"/api/v1/books/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
