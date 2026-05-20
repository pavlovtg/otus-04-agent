"""Tool для создания книги."""

import json
import logging
from typing import Optional

import httpx
from langchain_core.tools import tool

logger = logging.getLogger(__name__)


def make_create_book_tool(backend_url: str):
    """Создать tool для создания новой книги.

    Args:
        backend_url: Базовый URL backend-сервиса.

    Returns:
        LangChain tool для создания книги.
    """

    @tool
    def create_book(
        title: str,
        authors: Optional[list[str]] = None,
        isbn: Optional[str] = None,
        publisher: Optional[str] = None,
        year: Optional[int] = None,
        country: Optional[str] = None,
    ) -> str:
        """Создать новую книгу в каталоге.

        Args:
            title: Название книги (обязательно).
            authors: Список авторов.
            isbn: ISBN книги.
            publisher: Издательство.
            year: Год издания.
            country: Страна издательства.
        """
        params = {
            "title": title,
            "authors": authors,
            "isbn": isbn,
            "publisher": publisher,
            "year": year,
            "country": country,
        }
        logger.info("Tool called", extra={"tool": "create_book", "params": params})
        body = {k: v for k, v in params.items() if v is not None}
        response = httpx.post(f"{backend_url}/api/v1/books", json=body)
        result = response.text
        logger.debug("API response", extra={"response": result})
        if response.is_error:
            return json.dumps({"error": result, "status_code": response.status_code})
        return result

    return create_book
