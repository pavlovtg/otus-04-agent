# Реализация Python AI-агента на LangChain

## Роль

- Ты — Senior Python-разработчик, реализующий AI-агента на LangChain.

## Контекст

- Проект: AI-агент — натурально-языковая обёртка над HTTP API `BooksCatalog`.
- Архитектурные решения: `docs/adr/README.md` (ADR-010..020).
- API: `docs/contracts/openapi/backend/books.yaml` (5 операций: getBooks, getBookById, createBook, updateBook, deleteBook).
- LLM: локальная Ollama, модель `qwen3.5:4b`.
- Фреймворк: LangChain (Python 3.12).
- Зависимости: `pyproject.toml`.
- Конфигурация: `.env` (не коммитится), `.env.example` (коммитится).

## Задача

Реализовать AI-агента в `/apps/agent/` в следующей последовательности:

1. Создать структуру проекта агента.
2. Реализовать LangChain Tools для каждой операции API.
3. Реализовать агента с системным промтом и контрактом ответа.
4. Написать тесты.
5. Настроить GitHub Actions CI.
6. Дополнить `docker-compose.yml` сервисами агента и Ollama.
7. Дополнить корневой `README.md` инструкцией по запуску агента.

## Требования

### Структура проекта (`/apps/agent/`)

- `main.py` — точка входа, запуск из CLI: `python main.py "<запрос>"`.
- `agent.py` — сборка агента (LLM + tools + системный промт).
- `tools/` — по одному файлу на tool: `get_books.py`, `get_book_by_id.py`, `create_book.py`, `update_book.py`, `delete_book.py`.
- `prompts/system.md` — системный промт агента.
- `pyproject.toml` — зависимости (основные + `[dev]` для тестов).
- `.env.example` — пример конфигурации (`BACKEND_URL`, `OLLAMA_BASE_URL`).
- `Dockerfile` — образ агента.
- `tests/` — unit и integration тесты.

### LangChain Tools (ADR-015, ADR-017)

- Каждая операция API — отдельный `@tool`-декоратор в отдельном файле.
- HTTP-клиент: `httpx` (sync).
- Tool возвращает строку (JSON → str).
- Логировать вызов tool (имя операции, параметры) с уровнем `INFO` и ответ API с уровнем `DEBUG`.

### Агент (ADR-020)

- Тип: `create_tool_calling_agent` + `AgentExecutor` (sync, `invoke`).
- LLM: `ChatOllama(model="qwen3.5:4b", base_url=OLLAMA_BASE_URL)`.
- Системный промт (`/apps/agent/prompts/system.md`): роль (оператор API книг), ограничения (только операции с книгами), правила вызова tool, формат ответа.
- Контракт ответа (фиксированный текстовый формат):

  ```
  Status: success | error
  Action: <описание действия>
  Data: <результат API>
  Errors: <если есть>
  ```

### Логирование

- Формат: структурные JSON-логи в одну строку через `python-json-logger` (`pythonjsonlogger.jsonlogger.JsonFormatter`).
- Настроить в `main.py`: уровень `INFO` по умолчанию, каждая запись — одна строка JSON.
- Tools: `INFO` — вызов (поля `tool`, `params`); `DEBUG` — ответ API (поле `response`).
- Агент: `INFO` — входящий запрос (поле `query`); `INFO` — итоговый ответ (поле `answer`).
- Ошибки: перехватывать исключения в `main.py`, логировать с уровнем `ERROR` (поле `error`), выводить контракт ответа с `Status: error`.

### Тесты (ADR-012, ADR-018, ADR-019)

- Фреймворк: `pytest`.
- Unit-тесты: каждый tool тестируется с мок-HTTP через `pytest-httpx`.
- Integration-тесты: агент тестируется с `FakeListChatModel` + `pytest-httpx` (без реального Ollama и backend).
- Минимальный code coverage: 90%.
- Папка: `/apps/agent/tests/`.

### GitHub Actions CI (ADR-013)

- Файл: `.github/workflows/agent.yml`.
- Триггеры: `push` и `pull_request`.
- Python: `3.12`.
- Один job: `pip install -e .[dev]` → `pytest --cov` → coverage check (≥ 90%).
- При ошибке любого шага CI падает, PR блокируется.

### Docker Compose (ADR-016)

- Дополнить `infrastructure/docker-compose/docker-compose.yml`:
  - Сервис `ollama`: образ `ollama/ollama`, CPU-only, при старте выполняет `ollama pull qwen3.5:4b`.
  - Сервис `agent`: one-shot, собирается из `/apps/agent/Dockerfile`, зависит от `ollama` и `books-catalog`.
- `Dockerfile` агента: `/apps/agent/Dockerfile`.

### README

- Дополнить корневой `README.md`:
  - Раздел «Агент»: как настроить `.env` (скопировать `.env.example`, заполнить `BACKEND_URL` и `OLLAMA_BASE_URL`).
  - Запуск агента через docker compose: `docker compose run --rm agent "запрос пользователя"`.
  - Запуск агента локально: `cd apps/agent && python main.py "<запрос>"`.
  - Примеры запросов (минимум 3).

### Code style

- Следовать правилам из ADR-003 (`docs/adr/ADR-003-python-code-style.md`).
