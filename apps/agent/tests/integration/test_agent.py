"""Integration-тесты агента с кастомным FakeChatModel."""

from typing import Any, Iterator, Optional, Sequence

import pytest
from langchain_core.language_models import BaseChatModel
from langchain_core.messages import AIMessage, BaseMessage
from langchain_core.outputs import ChatGeneration, ChatResult
from langchain_core.runnables import Runnable
from langchain_core.tools import BaseTool

from agent import build_agent
from tools.create_book import make_create_book_tool
from tools.delete_book import make_delete_book_tool
from tools.get_book_by_id import make_get_book_by_id_tool
from tools.get_books import make_get_books_tool
from tools.update_book import make_update_book_tool

BACKEND_URL = "http://test-backend"

CONTRACT_RESPONSE = (
    "Status: success\n"
    "Action: получение списка книг\n"
    'Data: [{"id": "1", "title": "Война и мир"}]\n'
    "Errors: нет"
)

ERROR_RESPONSE = (
    "Status: error\n"
    "Action: получение книги\n"
    "Data: нет данных\n"
    "Errors: книга не найдена"
)


class FakeChatModelWithTools(BaseChatModel):
    """Fake chat model с поддержкой bind_tools для integration-тестов.

    Реализует bind_tools (возвращает self), что позволяет использовать
    create_tool_calling_agent без реального LLM.
    """

    responses: list[str]

    def _generate(
        self,
        messages: list[BaseMessage],
        stop: Optional[list[str]] = None,
        run_manager: Any = None,
        **kwargs: Any,
    ) -> ChatResult:
        """Вернуть первый заготовленный ответ."""
        content = self.responses[0] if self.responses else ""
        message = AIMessage(content=content)
        return ChatResult(generations=[ChatGeneration(message=message)])

    def bind_tools(
        self,
        tools: Sequence[Any],
        **kwargs: Any,
    ) -> Runnable:
        """Вернуть self — tools игнорируются в fake-режиме."""
        return self

    @property
    def _llm_type(self) -> str:
        """Тип модели."""
        return "fake-chat-with-tools"


@pytest.fixture
def agent_tools():
    """Создать список tools с тестовым backend URL."""
    return [
        make_get_books_tool(BACKEND_URL),
        make_get_book_by_id_tool(BACKEND_URL),
        make_create_book_tool(BACKEND_URL),
        make_update_book_tool(BACKEND_URL),
        make_delete_book_tool(BACKEND_URL),
    ]


def test_build_agent_returns_executor(agent_tools):
    """build_agent возвращает AgentExecutor."""
    from langchain_classic.agents import AgentExecutor

    llm = FakeChatModelWithTools(responses=[CONTRACT_RESPONSE])
    executor = build_agent(llm=llm, tools=agent_tools)
    assert isinstance(executor, AgentExecutor)


def test_agent_invoke_returns_contract_format(agent_tools):
    """AgentExecutor.invoke возвращает dict с ключом output в формате контракта."""
    llm = FakeChatModelWithTools(responses=[CONTRACT_RESPONSE])
    executor = build_agent(llm=llm, tools=agent_tools)
    result = executor.invoke({"input": "Покажи список книг"})
    assert isinstance(result, dict)
    assert "output" in result
    output = result["output"]
    assert "Status:" in output
    assert "Action:" in output
    assert "Data:" in output
    assert "Errors:" in output


def test_agent_invoke_success_status(agent_tools):
    """При успешном ответе LLM output содержит Status: success."""
    llm = FakeChatModelWithTools(responses=[CONTRACT_RESPONSE])
    executor = build_agent(llm=llm, tools=agent_tools)
    result = executor.invoke({"input": "Покажи список книг"})
    assert "Status: success" in result["output"]


def test_agent_invoke_error_status(agent_tools):
    """При ответе LLM с ошибкой output содержит Status: error."""
    llm = FakeChatModelWithTools(responses=[ERROR_RESPONSE])
    executor = build_agent(llm=llm, tools=agent_tools)
    result = executor.invoke({"input": "Найди книгу по ID"})
    assert "Status: error" in result["output"]
