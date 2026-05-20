"""Unit-тесты для main.py."""

import sys
from unittest.mock import MagicMock, patch

import pytest


def test_setup_logging_configures_json_handler():
    """_setup_logging настраивает JSON-форматтер на root logger."""
    import logging

    from main import _setup_logging

    _setup_logging()
    root = logging.getLogger()
    assert len(root.handlers) >= 1
    handler = root.handlers[0]
    from pythonjsonlogger import json as pythonjson

    assert isinstance(handler.formatter, pythonjson.JsonFormatter)


def test_main_no_args_exits(monkeypatch):
    """main() завершается с кодом 1 если нет аргументов."""
    monkeypatch.setenv("BACKEND_URL", "http://test")
    monkeypatch.setenv("OLLAMA_BASE_URL", "http://ollama")
    monkeypatch.setattr(sys, "argv", ["main.py"])

    with patch("main.build_agent"), patch("main.ChatOllama"):
        from main import main

        with pytest.raises(SystemExit) as exc_info:
            main()
        assert exc_info.value.code == 1


def test_main_success(monkeypatch, capsys):
    """main() выводит ответ агента при успешном выполнении."""
    monkeypatch.setenv("BACKEND_URL", "http://test")
    monkeypatch.setenv("OLLAMA_BASE_URL", "http://ollama")
    monkeypatch.setattr(sys, "argv", ["main.py", "Покажи книги"])

    mock_executor = MagicMock()
    mock_executor.invoke.return_value = {
        "output": "Status: success\nAction: тест\nData: []\nErrors: нет"
    }

    with patch("main.build_agent", return_value=mock_executor), patch(
        "main.ChatOllama"
    ):
        from main import main

        main()

    captured = capsys.readouterr()
    assert "Status: success" in captured.out


def test_main_agent_exception(monkeypatch, capsys):
    """main() выводит Status: error и завершается с кодом 1 при исключении."""
    monkeypatch.setenv("BACKEND_URL", "http://test")
    monkeypatch.setenv("OLLAMA_BASE_URL", "http://ollama")
    monkeypatch.setattr(sys, "argv", ["main.py", "Покажи книги"])

    mock_executor = MagicMock()
    mock_executor.invoke.side_effect = RuntimeError("connection refused")

    with patch("main.build_agent", return_value=mock_executor), patch(
        "main.ChatOllama"
    ):
        from main import main

        with pytest.raises(SystemExit) as exc_info:
            main()
        assert exc_info.value.code == 1

    captured = capsys.readouterr()
    assert "Status: error" in captured.out
    assert "connection refused" in captured.out
