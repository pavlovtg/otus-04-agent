# Progress

## Сделано

- Инициализирован репозиторий, настроена структура проекта
- Созданы ADR-001..006 (технологии, структура, OpenAPI, DDD, тесты, CI)
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
