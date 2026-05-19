# Реализация Backend API сервиса книг

## Роль

- Ты — Senior .NET разработчик, реализующий backend-сервис по принципам DDD и contract-first подхода.

## Контекст

- Проект: AI-агент на LangChain, которому нужен реальный HTTP API для вызова через tool.
- Архитектурные требования (index) в /docs/adr/README.md
- Хранение данных: in-memory репозиторий (без БД).
- Начальные данные: 10 книг из `src/BookService/books.json`, копируется в Docker-образ.

## Задача

Реализовать backend-сервис управления книгами в следующей последовательности:

1. Создать OpenAPI v3 спецификацию (`/docs/contracts/openapi/backend/books.yaml`).
2. Реализовать .NET сервис по спецификации с применением DDD.
3. Написать unit, integration и microservice тесты.
4. Упаковать сервис в Docker, настроить `docker-compose`.
5. Настроить CI через GitHub Actions.

## Требования

### Сущность Book

- `id` — GUID, технический первичный ключ, генерируется сервисом.
- `title` — полное название книги, обязательное.
- `authors` — список строк (`string[]`), один или несколько авторов, обязательное.
- `isbn` — уникальный международный стандартный книжный номер, обязательное.
- `publisher` — название издательства, обязательное.
- `year` — год издания, обязательное.
- `country` — страна издательства, необязательное.

### API

- Базовый URL: `/api/v1/books`.
- Операции: `GET /api/v1/books` (список), `GET /api/v1/books/{id}`, `POST /api/v1/books`, `PUT /api/v1/books/{id}`, `DELETE /api/v1/books/{id}`.
- Swagger UI: всегда включён (любое окружение).
- OpenAPI спецификация: формат YAML, версия 3.x, хранится в `/docs/contracts/openapi/backend/books.yaml`.

### Архитектура (DDD)

- Один .csproj, слои: `Domain/`, `Application/`, `Infrastructure/`, `Adapters/`.
- `Domain/` — сущность `Book`, интерфейс репозитория, доменные исключения.
- `Application/` — use cases (команды и запросы), DTO.
- `Infrastructure/` — инфраструктурные классы необходимые для работы сервиса, хендлеры и хелперы.
- `Adapters/` — HTTP-контроллеры, маппинг DTO ↔ HTTP-модели, in-memory реализация репозитория, загрузка `books.json`.
- Названия проекта `BooksCatalog`.

### Начальные данные

- Файл: `src/BooksCatalog/books.json`, 10 книг, сгенерировать реалистичные данные.
- Загрузка при старте приложения через `IHostedService` или `IStartupFilter`.
- Файл копируется в Docker-образ.

### Тесты (ADR-005)

- Unit-тесты: покрывают `Domain/` и `Application/` слои, моки через интерфейсы.
- Integration-тесты: HTTP-запросы через `WebApplicationFactory`, проверка статус-кодов и тела ответа.
- Microservice-тесты: `WebApplicationFactory` без моков внешних зависимостей, полный сценарий CRUD.
- Минимальный code coverage: 90%.
- Папка тестов: `/apps/backend/tests/`.
- Именование проекта с тестами: `<ProjectName>.Tests.csproj`. Где `<ProjectName>` — название проекта с исходным кодом.
- Фрейморк: `xUnit`.

### Docker

- `Dockerfile` в `/apps/backend/`.
- `docker-compose.yml` в `/infrastructure/docker-compose/`.
- Порт: `5000`.
- Файл `books.json` копируется в образ командой `COPY`.

### CI (ADR-006)

- GitHub Actions, файл `.github/workflows/backend.yml`.
- Триггеры: `push` и `pull_request`.
- Один job: build → test → coverage check.
- Шаги: `dotnet build`, `dotnet test --collect:"XPlat Code Coverage"`, проверка coverage ≥ 90%.
- При ошибке любого шага CI падает, PR блокируется.

### Code style

- Следовать правилам из `/docs/code-style/csharp.md`.
