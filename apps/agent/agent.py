"""Сборка AI-агента."""

import logging
from pathlib import Path

from langchain_classic.agents import AgentExecutor, create_tool_calling_agent
from langchain_core.language_models import BaseChatModel
from langchain_core.prompts import ChatPromptTemplate, MessagesPlaceholder
from langchain_core.tools import BaseTool

logger = logging.getLogger(__name__)

_SYSTEM_PROMPT_PATH = Path(__file__).parent / "prompts" / "system.md"


def _load_system_prompt() -> str:
    """Загрузить системный промт из файла."""
    return _SYSTEM_PROMPT_PATH.read_text(encoding="utf-8")


def build_agent(llm: BaseChatModel, tools: list[BaseTool]) -> AgentExecutor:
    """Собрать агента из LLM и tools.

    Args:
        llm: Языковая модель.
        tools: Список LangChain tools.

    Returns:
        Готовый AgentExecutor.
    """
    system_prompt = _load_system_prompt()
    prompt = ChatPromptTemplate.from_messages(
        [
            ("system", system_prompt),
            ("human", "{input}"),
            MessagesPlaceholder(variable_name="agent_scratchpad"),
        ]
    )
    agent = create_tool_calling_agent(llm=llm, tools=tools, prompt=prompt)
    return AgentExecutor(agent=agent, tools=tools, verbose=False)
