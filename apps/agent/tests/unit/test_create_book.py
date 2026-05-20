"""Unit-тесты для tool create_book."""

import json

from tools.create_book import make_create_book_tool

BACKEND_URL = "http://test-backend"


def test_create_book_success(httpx_mock):
    """Успешное создание возвращает JSON-строку с новой книгой."""
    book = {"id": "abc", "title": "Преступление и наказание"}
    httpx_mock.add_response(
        method="POST",
        url=f"{BACKEND_URL}/api/v1/books",
        json=book,
        status_code=201,
    )
    tool = make_create_book_tool(BACKEND_URL)
    result = tool.invoke({"title": "Преступление и наказание"})
    assert "Преступление и наказание" in result


def test_create_book_with_all_fields(httpx_mock):
    """Создание с полными данными передаёт все поля в запросе."""
    book = {
        "id": "abc",
        "title": "Идиот",
        "authors": ["Достоевский"],
        "isbn": "978-5-17-090000-0",
        "publisher": "АСТ",
        "year": 1869,
        "country": "Россия",
    }
    httpx_mock.add_response(
        method="POST",
        url=f"{BACKEND_URL}/api/v1/books",
        json=book,
        status_code=201,
    )
    tool = make_create_book_tool(BACKEND_URL)
    result = tool.invoke(
        {
            "title": "Идиот",
            "authors": ["Достоевский"],
            "isbn": "978-5-17-090000-0",
            "publisher": "АСТ",
            "year": 1869,
            "country": "Россия",
        }
    )
    assert "Идиот" in result


def test_create_book_validation_error(httpx_mock):
    """При 400 возвращает JSON с полем error."""
    httpx_mock.add_response(
        method="POST",
        url=f"{BACKEND_URL}/api/v1/books",
        status_code=400,
        text="Validation error",
    )
    tool = make_create_book_tool(BACKEND_URL)
    result = tool.invoke({"title": ""})
    data = json.loads(result)
    assert "error" in data
    assert data["status_code"] == 400
