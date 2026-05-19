# Фиксация архитектурных решений Backend

## Роль

- Ты — архитектор, фиксирующий принятые решения в системе ADR проекта.

## Контекст

- Проект: AI-агент на LangChain с backend-сервисом `BooksCatalog` на .NET.
- ADR хранятся в `docs/adr/`, индекс — `docs/adr/README.md`.
- Формат ADR: `# ADR-NNN: Название`, поля `Контекст` и `Решение` — одно предложение каждое.

## Задача

Создать следующие ADR и обновить индекс `docs/adr/README.md`.

## Требования

### ADR-002: Структура папок репозитория

- `/apps` — приложения и домены.
- `/apps/backend/` — домен backend-сервиса.
- `/apps/backend/Backend.slnx` — solution backend.
- `/apps/backend/src/` — исходный код backend-сервиса.
- `/apps/backend/tests/` — тесты backend-сервиса.
- `/infrastructure/docker-compose/` — файлы для развёртывания через docker-compose.
- `/docs/contracts/openapi/` — OpenAPI-спецификации, разделённые по доменам и сервисам.

### ADR-003: Contract-first с OpenAPI v3

- Подход: contract-first — сначала создаётся OpenAPI v3 спецификация в формате YAML, затем реализуется сервис.
- Спецификации хранятся в `/docs/contracts/openapi/`.

### ADR-004: Domain Driven Design для backend-сервиса

- Архитектура: DDD с разделением на слои внутри одного .csproj.
- Слои: `Domain/`, `Application/`, `Infrastructure/`, `Adapters/`.

### ADR-005: Стратегия тестирования backend-сервиса

- Виды тестов: unit (domain/application), integration (HTTP через `WebApplicationFactory`), microservice (`WebApplicationFactory` без моков внешних зависимостей).
- Минимальный code coverage: 90%; CI падает при нарушении.
- Фреймворк: xUnit.
- Нейминг тестового проекта: `<ProjectName>.Tests.csproj`, где `<ProjectName>` — название тестируемого проекта.

### ADR-006: CI через GitHub Actions

- Один job: build → test → coverage check (≥ 90%).
- Триггеры: `push` и `pull_request`.
- При ошибке любого шага CI падает, PR блокируется.
