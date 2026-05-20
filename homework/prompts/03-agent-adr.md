# Фиксация архитектурных решений AI-агента

## Роль

- Ты — архитектор, фиксирующий принятые решения в системе ADR проекта.

## Контекст

- Проект: AI-агент на LangChain с backend-сервисом `BooksCatalog` на .NET.
- ADR хранятся в `docs/adr/`, индекс — `docs/adr/README.md`.
- Формат ADR: `# ADR-NNN: Название`, поля `Контекст` и `Решение` — одно предложение каждое.

## Задача

Обновить ADR-005 и создать ADR-010..016, обновить индекс `docs/adr/README.md`.

## Требования

### ADR-005: Структура папок репозитория (обновить)

- Добавить: `/apps/agent/` — домен AI-агента.

### ADR-010: Python для AI-агента

- Язык реализации агента — Python.

### ADR-011: LangChain как фреймворк агента

- Фреймворк: LangChain (Python).
- Документация: [LangChain Python Overview](https://docs.langchain.com/oss/python/langchain/overview)

### ADR-012: Стратегия тестирования агента

- Виды тестов: unit и integration.
- Фреймворк: pytest.
- Минимальный code coverage: 90%; CI падает при нарушении.

### ADR-013: CI для агента

- Отдельный GitHub Actions workflow для агента (независимо от backend).
- Один job: test → coverage check (≥ 90%).
- Триггеры: `push` и `pull_request`.
- При ошибке любого шага CI падает, PR блокируется.

### ADR-014: LLM-провайдер агента

- LLM-провайдер: локальная Ollama с моделью `qwen3.5:4b`.

### ADR-015: Интеграция с API backend через LangChain Tools

- Каждая операция API backend (get/create/update/delete) реализуется отдельным LangChain Tool.

### ADR-016: Ollama в docker-compose

- Ollama запускается как отдельный сервис в docker-compose.
- Модель `qwen3.5:4b` загружается через `ollama pull` при старте контейнера.
