"""Tool для получения книги по ID."""

import json
import logging

import httpx
from langchain_core.tools import tool

logger = logging.getLogger(__name__)


def make_get_book_by_id_tool(backend_url: str):
    """Создать tool для получения книги по ID.

    Args:
        backend_url: Базовый URL backend-сервиса.

    Returns:
        LangChain tool для получения книги по ID.
    """

    @tool
    def get_book_by_id(book_id: str) -> str:
        """Получить книгу по её уникальному идентификатору (UUID).

        Args:
            book_id: UUID книги.
        """
        logger.info(
            "Tool called",
            extra={"tool": "get_book_by_id", "params": {"book_id": book_id}},
        )
        response = httpx.get(f"{backend_url}/api/v1/books/{book_id}")
        result = response.text
        logger.debug("API response", extra={"response": result})
        if response.is_error:
            return json.dumps({"error": result, "status_code": response.status_code})
        return result

    return get_book_by_id
