"""Unit-тесты для tool delete_book."""

import json

from tools.delete_book import make_delete_book_tool

BACKEND_URL = "http://test-backend"
BOOK_ID = "123e4567-e89b-12d3-a456-426614174000"


def test_delete_book_success(httpx_mock):
    """Успешное удаление возвращает JSON со статусом deleted."""
    httpx_mock.add_response(
        method="DELETE",
        url=f"{BACKEND_URL}/api/v1/books/{BOOK_ID}",
        status_code=204,
        text="",
    )
    tool = make_delete_book_tool(BACKEND_URL)
    result = tool.invoke({"book_id": BOOK_ID})
    data = json.loads(result)
    assert data["status"] == "deleted"
    assert data["book_id"] == BOOK_ID


def test_delete_book_not_found(httpx_mock):
    """При 400 возвращает JSON с полем error."""
    httpx_mock.add_response(
        method="DELETE",
        url=f"{BACKEND_URL}/api/v1/books/{BOOK_ID}",
        status_code=400,
        text="Not found",
    )
    tool = make_delete_book_tool(BACKEND_URL)
    result = tool.invoke({"book_id": BOOK_ID})
    data = json.loads(result)
    assert "error" in data
    assert data["status_code"] == 400
