"""Tool для удаления книги."""

import json
import logging

import httpx
from langchain_core.tools import tool

logger = logging.getLogger(__name__)


def make_delete_book_tool(backend_url: str):
    """Создать tool для удаления книги.

    Args:
        backend_url: Базовый URL backend-сервиса.

    Returns:
        LangChain tool для удаления книги.
    """

    @tool
    def delete_book(book_id: str) -> str:
        """Удалить книгу из каталога по её уникальному идентификатору.

        Args:
            book_id: UUID книги.
        """
        logger.info(
            "Tool called",
            extra={"tool": "delete_book", "params": {"book_id": book_id}},
        )
        response = httpx.delete(f"{backend_url}/api/v1/books/{book_id}")
        logger.debug("API response", extra={"response": response.text})
        if response.is_error:
            return json.dumps(
                {"error": response.text, "status_code": response.status_code}
            )
        return json.dumps({"status": "deleted", "book_id": book_id})

    return delete_book
