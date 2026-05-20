# Progress

## Сделано

- Инициализирован репозиторий, настроена структура проекта
- Созданы ADR-001..009:
  - ADR-001: Code style для C#
  - ADR-002: Code style для Markdown
  - ADR-003: Code style для Python
  - ADR-004: .NET для backend-сервиса
  - ADR-005: Структура папок репозитория
  - ADR-006: Contract-first с OpenAPI v3
  - ADR-007: Domain Driven Design для backend-сервиса
  - ADR-008: Стратегия тестирования backend-сервиса
  - ADR-009: CI через GitHub Actions
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

- Python AI-агент на LangChain
