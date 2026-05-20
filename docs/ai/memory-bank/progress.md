# Progress

## Сделано

- Инициализирован репозиторий, настроена структура проекта
- Созданы ADR-001..016:
  - ADR-001: Code style для C#
  - ADR-002: Code style для Markdown
  - ADR-003: Code style для Python
  - ADR-004: .NET для backend-сервиса
  - ADR-005: Структура папок репозитория (включая `/apps/agent/`)
  - ADR-006: Contract-first с OpenAPI v3
  - ADR-007: Domain Driven Design для backend-сервиса
  - ADR-008: Стратегия тестирования backend-сервиса
  - ADR-009: CI через GitHub Actions
  - ADR-010: Python для AI-агента
  - ADR-011: LangChain как фреймворк агента
  - ADR-012: Стратегия тестирования агента
  - ADR-013: CI для агента
  - ADR-014: LLM-провайдер агента
  - ADR-015: Интеграция с API backend через LangChain Tools
  - ADR-016: Ollama в docker-compose
  - ADR-017: httpx как HTTP-клиент агента
  - ADR-018: pytest-httpx для мока HTTP в тестах агента
  - ADR-019: FakeListChatModel для изоляции LLM в тестах агента
  - ADR-020: Sync-режим агента
- Создана OpenAPI спецификация `books.yaml`
- Реализован backend-сервис `BooksCatalog` (.NET 10, DDD):
  - Domain, Application, Infrastructure, Adapters слои
  - In-memory репозиторий
  - Загрузка 10 книг из `books.json` при старте
  - Swagger UI на `/api/swagger`
  - `InternalControllerFeatureProvider` для `internal` контроллеров
- Написаны тесты (20 штук, все проходят):
  - Unit (Domain), Integration (HTTP), Microservice (CRUD)
- Dockerfile, docker-compose.yml, GitHub Actions CI

## В работе

- Нет

## Не начато

- Реализовать Python AI-агент на LangChain в `/apps/agent/` (промт `04-agent-implementation.md`)
