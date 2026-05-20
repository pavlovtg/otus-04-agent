"""Unit-тесты для tool get_books."""

import json

import pytest
import httpx

from tools.get_books import make_get_books_tool

BACKEND_URL = "http://test-backend"


@pytest.fixture
def get_books():
    """Создать tool get_books с тестовым backend URL."""
    return make_get_books_tool(BACKEND_URL)


def test_get_books_success(httpx_mock):
    """Успешный запрос возвращает JSON-строку со списком книг."""
    books = [{"id": "1", "title": "Война и мир"}]
    httpx_mock.add_response(
        method="GET",
        url=f"{BACKEND_URL}/api/v1/books",
        json=books,
        status_code=200,
    )
    tool = make_get_books_tool(BACKEND_URL)
    result = tool.invoke({})
    assert "Война и мир" in result


def test_get_books_error(httpx_mock):
    """При HTTP-ошибке возвращает JSON с полем error."""
    httpx_mock.add_response(
        method="GET",
        url=f"{BACKEND_URL}/api/v1/books",
        status_code=500,
        text="Internal Server Error",
    )
    tool = make_get_books_tool(BACKEND_URL)
    result = tool.invoke({})
    data = json.loads(result)
    assert "error" in data
    assert data["status_code"] == 500
