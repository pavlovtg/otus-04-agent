"""Точка входа агента. Запуск: python main.py "<запрос>"."""

import logging
import os
import sys

from dotenv import load_dotenv
from langchain_ollama import ChatOllama  # noqa: F401 (used at runtime)
from pythonjsonlogger import json as pythonjson

from agent import build_agent
from tools.create_book import make_create_book_tool
from tools.delete_book import make_delete_book_tool
from tools.get_book_by_id import make_get_book_by_id_tool
from tools.get_books import make_get_books_tool
from tools.update_book import make_update_book_tool


def _setup_logging() -> None:
    """Настроить структурное JSON-логирование."""
    handler = logging.StreamHandler()
    formatter = pythonjson.JsonFormatter(
        fmt="%(asctime)s %(levelname)s %(name)s %(message)s",
        json_ensure_ascii=False,
    )
    handler.setFormatter(formatter)
    root = logging.getLogger()
    root.handlers.clear()
    root.addHandler(handler)
    root.setLevel(logging.INFO)


def main() -> None:
    """Запустить агента с запросом из аргументов командной строки."""
    _setup_logging()
    logger = logging.getLogger(__name__)

    load_dotenv()
    backend_url = os.environ["BACKEND_URL"]
    ollama_base_url = os.environ["OLLAMA_BASE_URL"]

    if len(sys.argv) < 2:
        print("Использование: python main.py \"<запрос>\"")
        sys.exit(1)

    query = sys.argv[1]
    logger.info("Incoming query", extra={"query": query})

    tools = [
        make_get_books_tool(backend_url),
        make_get_book_by_id_tool(backend_url),
        make_create_book_tool(backend_url),
        make_update_book_tool(backend_url),
        make_delete_book_tool(backend_url),
    ]

    llm = ChatOllama(model="qwen3.5:4b", base_url=ollama_base_url)
    executor = build_agent(llm=llm, tools=tools)

    try:
        result = executor.invoke({"input": query})
        answer = result["output"]
        logger.info("Agent answer", extra={"answer": answer})
        print(answer)
    except Exception as exc:
        logger.error("Agent error", extra={"error": str(exc)})
        print(
            f"Status: error\n"
            f"Action: выполнение запроса\n"
            f"Data: нет данных\n"
            f"Errors: {exc}"
        )
        sys.exit(1)


if __name__ == "__main__":
    main()
