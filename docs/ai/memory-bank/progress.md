# Progress

## Сделано

- Инициализирован репозиторий, настроена структура проекта
- Созданы ADR-001..020
- Создана OpenAPI спецификация `books.yaml`
- Реализован backend-сервис `BooksCatalog` (.NET 10, DDD):
  - Domain, Application, Infrastructure, Adapters слои
  - In-memory репозиторий
  - Загрузка 10 книг из `books.json` при старте
  - Swagger UI на `/api/swagger`
- Написаны тесты backend (20 штук, все проходят)
- Dockerfile, docker-compose.yml, GitHub Actions CI для backend
- Реализован Python AI-агент в `/apps/agent/`:
  - 5 LangChain Tools (get_books, get_book_by_id, create_book, update_book, delete_book)
  - `agent.py`: `build_agent(llm, tools)` через `langchain_classic`
  - `main.py`: CLI, JSON-логирование, обработка ошибок
  - `prompts/system.md`: системный промт
  - 20 тестов (unit + integration), coverage 99.31%
  - GitHub Actions CI (`.github/workflows/agent.yml`)
  - Dockerfile агента (`python:3.12-slim`)
  - docker-compose: сервисы `ollama` и `agent`
  - README: раздел «Агент» с инструкцией и примерами

## В работе

- Нет

## Не начато

- Нет
