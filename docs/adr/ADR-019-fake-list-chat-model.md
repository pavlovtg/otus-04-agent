# ADR-019: FakeListChatModel для изоляции LLM в тестах агента

- **Контекст**: необходимо изолировать LLM в integration-тестах агента без реального Ollama.
- **Решение**: заменять LLM на `FakeListChatModel` из `langchain_core` в integration-тестах агента.
