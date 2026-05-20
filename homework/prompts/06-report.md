# Отчёт о выполненной домашней работе

## Роль

- Ты — Senior Python-разработчик, оформляющий отчёт о выполненной домашней работе по курсу OTUS.

## Контекст

- Задание: `homework/assignment.md`.
- Проект: AI-агент на LangChain — натурально-языковая обёртка над HTTP API `BooksCatalog`.
- LLM: локальная Ollama, модель `qwen3.5:4b`.
- Агент: `apps/agent/main.py`, сборка — `apps/agent/agent.py`.
- Tools: `apps/agent/tools/` (5 файлов: `get_books.py`, `get_book_by_id.py`, `create_book.py`, `update_book.py`, `delete_book.py`).
- Системный промт: `apps/agent/prompts/system.md`.
- Конфигурация: `apps/agent/.env.example` (`BACKEND_URL`, `OLLAMA_BASE_URL`).
- Скрипты запуска: `setup.sh`, `run.sh`.
- Использованные промты: `homework/prompts/00-init-repository.md` .. `homework/prompts/06-report.md`.

## Задача

1. Запустить агента с 5 тестовыми запросами через `./run.sh "<запрос>"`.
2. Зафиксировать реальные ответы агента и фрагменты JSON-логов.
3. Создать файл `homework/report.md` с заполненными разделами.

## Требования

### Структура `homework/report.md`

#### 1. LLM и настройка

- Указать: LLM — Ollama, модель `qwen3.5:4b`.
- Настройка: скопировать `apps/agent/.env.example` → `apps/agent/.env`, заполнить `BACKEND_URL` и `OLLAMA_BASE_URL`.
- Запуск Ollama на хосте: `ollama serve` (или через `setup.sh`).

#### 2. API и операции

- API: `BooksCatalog` (HTTP REST).
- OpenAPI-спецификация: `docs/contracts/openapi/backend/books.yaml`.
- Поддерживаемые операции:

  | Операция       | Tool-файл                            | HTTP-метод | Путь                  |
  |----------------|--------------------------------------|------------|-----------------------|
  | Список книг    | `apps/agent/tools/get_books.py`      | GET        | `/api/v1/books`       |
  | Книга по ID    | `apps/agent/tools/get_book_by_id.py` | GET        | `/api/v1/books/{id}`  |
  | Создать книгу  | `apps/agent/tools/create_book.py`    | POST       | `/api/v1/books`       |
  | Обновить книгу | `apps/agent/tools/update_book.py`    | PUT        | `/api/v1/books/{id}`  |
  | Удалить книгу  | `apps/agent/tools/delete_book.py`    | DELETE     | `/api/v1/books/{id}`  |

#### 3. Запуск агента

- Подготовка окружения (один раз): `./setup.sh`
- Запуск агента: `./run.sh "<запрос пользователя>"`
- Запуск через docker compose напрямую:
  ```bash
  docker compose -f infrastructure/docker-compose/docker-compose.yml run --rm agent "<запрос>"
  ```
- Запуск локально:
  ```bash
  cd apps/agent && python main.py "<запрос>"
  ```

#### 4. Подтверждение реализации

- **Tool с реальным вызовом API**: `apps/agent/tools/get_books.py:L22–L31` — объявление `@tool`, HTTP-вызов `httpx.get` на L26.
- **Логирование вызова tool**: `apps/agent/tools/get_books.py:L25` — `logger.info("Tool called", ...)` уровень `INFO`; L28 — `logger.debug("API response", ...)` уровень `DEBUG`.
- **Контракт ответа агента**: `apps/agent/prompts/system.md:L22–L29` — раздел «Формат ответа» (Status / Action / Data / Errors).
- **Пример запроса → tool**: запрос «Покажи все книги» → вызов tool `get_books` → `GET /api/v1/books`.
- **Фрагмент лога**: вставить реальный JSON-лог из вывода `./run.sh` (поля `tool`, `params`, `response`).

#### 5. Тестовые запросы (минимум 5)

Выполнить следующие запросы и вставить реальные ответы агента:

1. `./run.sh "Покажи все книги"`
2. `./run.sh "Покажи книгу с ID 1"`
3. `./run.sh "Создай книгу с названием 'Чистый код' автора Роберт Мартин"`
4. `./run.sh "Обнови название книги с ID 1 на 'Clean Code'"`
5. `./run.sh "Удали книгу с ID 1"`

Для каждого запроса зафиксировать:
- Запрос пользователя
- Ответ агента (в формате контракта: Status / Action / Data / Errors)
- Фрагмент JSON-лога с вызовом tool (поле `tool`)

#### 6. Использованные промпты

- `homework/prompts/00-init-repository.md` — инициализация репозитория
- `homework/prompts/01-backend-adr.md` — ADR для backend
- `homework/prompts/02-backend-api.md` — реализация backend API
- `homework/prompts/03-agent-adr.md` — ADR для агента
- `homework/prompts/04-agent-implementation.md` — реализация агента
- `homework/prompts/05-scripts.md` — скрипты подготовки и запуска
- `homework/prompts/06-report.md` — отчёт о выполненной работе
- `apps/agent/prompts/system.md` — системный промт агента

### Общие требования к `report.md`

- Формат: Markdown.
- Язык: русский.
- Секреты не упоминать (ключи API, токены).
- Разделы «Подтверждение реализации» и «Тестовые запросы» заполнить реальными данными после запуска агента.
