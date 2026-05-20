"""Tool для получения списка книг."""

import json
import logging

import httpx
from langchain_core.tools import tool

logger = logging.getLogger(__name__)


def make_get_books_tool(backend_url: str):
    """Создать tool для получения списка книг.

    Args:
        backend_url: Базовый URL backend-сервиса.

    Returns:
        LangChain tool для получения списка книг.
    """

    @tool
    def get_books() -> str:
        """Получить список всех книг из каталога."""
        logger.info("Tool called", extra={"tool": "get_books", "params": {}})
        response = httpx.get(f"{backend_url}/api/v1/books")
        result = response.text
        logger.debug("API response", extra={"response": result})
        if response.is_error:
            return json.dumps({"error": result, "status_code": response.status_code})
        return result

    return get_books
