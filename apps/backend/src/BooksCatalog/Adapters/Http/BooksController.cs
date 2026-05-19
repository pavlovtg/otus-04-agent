using BooksCatalog.Application.Dto;
using BooksCatalog.Application.Services;
using BooksCatalog.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BooksCatalog.Adapters.Http;

[ApiController]
[Route("api/v1/books")]
internal class BooksController : ControllerBase
{
    private readonly BookService _bookService;
    private readonly ILogger<BooksController> _logger;

    public BooksController(BookService bookService, ILogger<BooksController> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<BookResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var books = new List<BookResponse>();

        await foreach (var dto in _bookService.GetAllAsync(cancellationToken))
        {
            books.Add(MapToResponse(dto));
        }

        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<BookResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var dto = await _bookService.GetByIdAsync(id, cancellationToken);
            return Ok(MapToResponse(dto));
        }
        catch (BookNotFoundException ex)
        {
            _logger.LogWarning(ex, "Book not found {BookId}", id);
            return BadRequest(new ProblemDetails { Detail = ex.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType<BookResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBookRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var dto = await _bookService.CreateAsync(MapToCreateDto(request), cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, MapToResponse(dto));
        }
        catch (BookValidationException ex)
        {
            _logger.LogWarning(ex, "Book validation failed");
            return BadRequest(new ProblemDetails { Detail = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<BookResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBookRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var dto = await _bookService.UpdateAsync(id, MapToUpdateDto(request), cancellationToken);
            return Ok(MapToResponse(dto));
        }
        catch (BookNotFoundException ex)
        {
            _logger.LogWarning(ex, "Book not found {BookId}", id);
            return BadRequest(new ProblemDetails { Detail = ex.Message });
        }
        catch (BookValidationException ex)
        {
            _logger.LogWarning(ex, "Book validation failed {BookId}", id);
            return BadRequest(new ProblemDetails { Detail = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _bookService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (BookNotFoundException ex)
        {
            _logger.LogWarning(ex, "Book not found {BookId}", id);
            return BadRequest(new ProblemDetails { Detail = ex.Message });
        }
    }

    private static BookResponse MapToResponse(BookDto dto)
    {
        return new BookResponse(
            dto.Id,
            dto.Title,
            dto.Authors,
            dto.Isbn,
            dto.Publisher,
            dto.Year,
            dto.Country);
    }

    private static CreateBookDto MapToCreateDto(CreateBookRequest request)
    {
        return new CreateBookDto(
            request.Title,
            request.Authors,
            request.Isbn,
            request.Publisher,
            request.Year,
            request.Country);
    }

    private static UpdateBookDto MapToUpdateDto(UpdateBookRequest request)
    {
        return new UpdateBookDto(
            request.Title,
            request.Authors,
            request.Isbn,
            request.Publisher,
            request.Year,
            request.Country);
    }
}
