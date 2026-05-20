# Реализация Backend API сервиса книг

## Роль

- Ты — Senior .NET разработчик, реализующий backend-сервис по принципам DDD и contract-first подхода.

## Контекст

- Проект: AI-агент на LangChain, которому нужен реальный HTTP API для вызова через tool.
- Архитектурные требования (index) в `/docs/adr/README.md`.
- Хранение данных: in-memory репозиторий (без БД).
- Начальные данные: 10 книг из `src/BooksCatalog/books.json`, копируется в Docker-образ.

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
- Swagger UI: всегда включён (любое окружение), маршрут `/api/swagger`.
- OpenAPI спецификация: формат YAML, версия 3.x, хранится в `/docs/contracts/openapi/backend/books.yaml`.

### Архитектура (DDD)

- Один .csproj, слои: `Domain/`, `Application/`, `Infrastructure/`, `Adapters/`.
- Все классы сервиса — `internal`.
- `Domain/` — сущность `Book`, интерфейс репозитория, доменные исключения.
- `Application/` — use cases (команды и запросы), DTO.
- `Infrastructure/` — инфраструктурные классы: `BooksCatalogLoader` (загрузка начальных данных), `InternalControllerFeatureProvider` (регистрация `internal` контроллеров).
- `Adapters/` — HTTP-контроллеры, маппинг DTO ↔ HTTP-модели (статические методы в контроллере), in-memory реализация репозитория.
- Название проекта: `BooksCatalog`.

### Логирование

- Формат: структурные JSON-логи в одну строку через `builder.Logging.AddJsonConsole`.
- `BookService` — логировать доменные операции `Create`, `Update`, `Delete` с уровнем `Information`, включать `BookId` и `BookTitle` в структурированные поля.
- `BooksController` — логировать отловленные доменные исключения (`BookNotFoundException`, `BookValidationException`) с уровнем `Warning`.
- Неотловленные исключения — перехватывать через `UseExceptionHandler`, логировать с уровнем `Error`, возвращать `500 Internal Server Error`.

### Подходы в коде

- `Book` — фабричный метод `Book.Create(...)`, приватный конструктор, валидация внутри сущности.
- Контроллер перехватывает доменные исключения (`BookNotFoundException`, `BookValidationException`) и возвращает `400 BadRequest` с `ProblemDetails`.
- `GetAll` использует `IAsyncEnumerable` (`await foreach`).
- `InternalControllerFeatureProvider` наследует `ControllerFeatureProvider`, переопределяет `IsController` для поддержки `internal` классов; регистрируется через `ConfigureApplicationPartManager`.

### Начальные данные

- Файл: `src/BooksCatalog/books.json`, 10 книг на русском языке с русскими авторами.
- Загрузка при старте приложения через `IHostedService`.
- Файл копируется в Docker-образ.

### Тесты (ADR-008)

- Unit-тесты: покрывают только `Domain/` слой, моки через интерфейсы.
- Integration-тесты: HTTP-запросы через `WebApplicationFactory`, проверка статус-кодов и тела ответа.
- Microservice-тесты: `WebApplicationFactory` без моков внешних зависимостей, полный сценарий CRUD + проверка доступности Swagger UI (`GET /api/swagger/index.html` → 200 OK).
- Минимальный code coverage: 90%.
- Папка тестов: `/apps/backend/tests/`.
- Именование проекта с тестами: `<ProjectName>.Tests.csproj`. Где `<ProjectName>` — название проекта с исходным кодом.
- Фреймворк: `xUnit`.

### Docker

- `Dockerfile` в `/apps/backend/`.
- `docker-compose.yml` в `/infrastructure/docker-compose/`.
- Порт: `5000`.
- Файл `books.json` копируется в образ командой `COPY`.

### CI (ADR-009)

- GitHub Actions, файл `.github/workflows/backend.yml`.
- Триггеры: `push` и `pull_request`.
- Один job: build → test → coverage check.
- Шаги: `dotnet build`, `dotnet test --collect:"XPlat Code Coverage"`, проверка coverage ≥ 90%.
- При ошибке любого шага CI падает, PR блокируется.

### README

- Добавить в корневой `README.md` инструкцию по запуску через Docker Compose.
- Указать ссылку на Swagger UI.
- Перечислить доступные API эндпоинты.
- Запуск только через Docker Compose (не через `dotnet run`).

### Code style

- Следовать правилам из [ADR-001](../adr/ADR-001-csharp-code-style.md).
