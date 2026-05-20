"""Unit-тесты для tool update_book."""

import json

from tools.update_book import make_update_book_tool

BACKEND_URL = "http://test-backend"
BOOK_ID = "123e4567-e89b-12d3-a456-426614174000"


def test_update_book_success(httpx_mock):
    """Успешное обновление возвращает JSON-строку с обновлённой книгой."""
    book = {"id": BOOK_ID, "title": "Новое название"}
    httpx_mock.add_response(
        method="PUT",
        url=f"{BACKEND_URL}/api/v1/books/{BOOK_ID}",
        json=book,
        status_code=200,
    )
    tool = make_update_book_tool(BACKEND_URL)
    result = tool.invoke({"book_id": BOOK_ID, "title": "Новое название"})
    assert "Новое название" in result


def test_update_book_not_found(httpx_mock):
    """При 400 возвращает JSON с полем error."""
    httpx_mock.add_response(
        method="PUT",
        url=f"{BACKEND_URL}/api/v1/books/{BOOK_ID}",
        status_code=400,
        text="Not found",
    )
    tool = make_update_book_tool(BACKEND_URL)
    result = tool.invoke({"book_id": BOOK_ID, "title": "Название"})
    data = json.loads(result)
    assert "error" in data
    assert data["status_code"] == 400


def test_update_book_excludes_book_id_from_body(httpx_mock):
    """book_id не попадает в тело запроса."""
    book = {"id": BOOK_ID, "title": "Обновлено", "year": 2024}
    httpx_mock.add_response(
        method="PUT",
        url=f"{BACKEND_URL}/api/v1/books/{BOOK_ID}",
        json=book,
        status_code=200,
    )
    tool = make_update_book_tool(BACKEND_URL)
    result = tool.invoke({"book_id": BOOK_ID, "title": "Обновлено", "year": 2024})
    assert "Обновлено" in result
