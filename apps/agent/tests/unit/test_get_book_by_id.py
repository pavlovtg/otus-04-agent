"""Unit-тесты для tool get_book_by_id."""

import json

from tools.get_book_by_id import make_get_book_by_id_tool

BACKEND_URL = "http://test-backend"
BOOK_ID = "123e4567-e89b-12d3-a456-426614174000"


def test_get_book_by_id_success(httpx_mock):
    """Успешный запрос возвращает JSON-строку с книгой."""
    book = {"id": BOOK_ID, "title": "Мастер и Маргарита"}
    httpx_mock.add_response(
        method="GET",
        url=f"{BACKEND_URL}/api/v1/books/{BOOK_ID}",
        json=book,
        status_code=200,
    )
    tool = make_get_book_by_id_tool(BACKEND_URL)
    result = tool.invoke({"book_id": BOOK_ID})
    assert "Мастер и Маргарита" in result


def test_get_book_by_id_not_found(httpx_mock):
    """При 400 возвращает JSON с полем error."""
    httpx_mock.add_response(
        method="GET",
        url=f"{BACKEND_URL}/api/v1/books/{BOOK_ID}",
        status_code=400,
        text="Not found",
    )
    tool = make_get_book_by_id_tool(BACKEND_URL)
    result = tool.invoke({"book_id": BOOK_ID})
    data = json.loads(result)
    assert "error" in data
    assert data["status_code"] == 400
