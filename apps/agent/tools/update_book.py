"""Tool для обновления книги."""

import json
import logging
from typing import Optional

import httpx
from langchain_core.tools import tool

logger = logging.getLogger(__name__)


def make_update_book_tool(backend_url: str):
    """Создать tool для обновления книги.

    Args:
        backend_url: Базовый URL backend-сервиса.

    Returns:
        LangChain tool для обновления книги.
    """

    @tool
    def update_book(
        book_id: str,
        title: str,
        authors: Optional[list[str]] = None,
        isbn: Optional[str] = None,
        publisher: Optional[str] = None,
        year: Optional[int] = None,
        country: Optional[str] = None,
    ) -> str:
        """Обновить существующую книгу в каталоге.

        Args:
            book_id: UUID книги.
            title: Новое название книги (обязательно).
            authors: Список авторов.
            isbn: ISBN книги.
            publisher: Издательство.
            year: Год издания.
            country: Страна издательства.
        """
        params = {
            "book_id": book_id,
            "title": title,
            "authors": authors,
            "isbn": isbn,
            "publisher": publisher,
            "year": year,
            "country": country,
        }
        logger.info("Tool called", extra={"tool": "update_book", "params": params})
        body = {k: v for k, v in params.items() if v is not None and k != "book_id"}
        response = httpx.put(f"{backend_url}/api/v1/books/{book_id}", json=body)
        result = response.text
        logger.debug("API response", extra={"response": result})
        if response.is_error:
            return json.dumps({"error": result, "status_code": response.status_code})
        return result

    return update_book
