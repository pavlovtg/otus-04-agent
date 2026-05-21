# Отчёт о выполненной домашней работе

## 1. LLM и настройка

- **LLM**: Ollama, модель `qwen3.5:4b`
- **Настройка**: скопировать `apps/agent/.env.example` → `apps/agent/.env`, заполнить `BACKEND_URL` и `OLLAMA_BASE_URL`
- **Запуск Ollama на хосте**: `ollama serve` (или через `./setup.sh`)

Пример `.env`:
```
BACKEND_URL=http://books-catalog:5000
OLLAMA_BASE_URL=http://host.docker.internal:11434
```

## 2. API и операции

- **API**: BooksCatalog (HTTP REST)
- **OpenAPI-спецификация**: [`docs/contracts/openapi/backend/books.yaml`](../docs/contracts/openapi/backend/books.yaml)

| Операция       | Tool-файл                            | HTTP-метод | Путь                  |
|----------------|--------------------------------------|------------|-----------------------|
| Список книг    | `apps/agent/tools/get_books.py`      | GET        | `/api/v1/books`       |
| Книга по ID    | `apps/agent/tools/get_book_by_id.py` | GET        | `/api/v1/books/{id}`  |
| Создать книгу  | `apps/agent/tools/create_book.py`    | POST       | `/api/v1/books`       |
| Обновить книгу | `apps/agent/tools/update_book.py`    | PUT        | `/api/v1/books/{id}`  |
| Удалить книгу  | `apps/agent/tools/delete_book.py`    | DELETE     | `/api/v1/books/{id}`  |

## 3. Запуск агента

Подготовка окружения (один раз):
```bash
./setup.sh
```

Запуск агента:
```bash
./run.sh "<запрос пользователя>"
```

Запуск через docker compose напрямую:
```bash
docker compose -f infrastructure/docker-compose/docker-compose.yml run --rm agent "<запрос>"
```

Запуск локально:
```bash
cd apps/agent && python main.py "<запрос>"
```

## 4. Подтверждение реализации

- **Tool с реальным вызовом API**: [`apps/agent/tools/get_books.py:L22–L31`](../apps/agent/tools/get_books.py#L22-L31) — объявление `@tool`, HTTP-вызов `httpx.get` на L26.
- **Логирование вызова tool**: [`apps/agent/tools/get_books.py:L25`](../apps/agent/tools/get_books.py#L25) — `logger.info("Tool called", ...)` уровень `INFO`; L28 — `logger.debug("API response", ...)` уровень `DEBUG`.
- **Контракт ответа агента**: [`apps/agent/prompts/system.md:L22–L29`](../apps/agent/prompts/system.md#L22-L29) — раздел «Формат ответа» (Status / Action / Data / Errors).
- **Пример запроса → tool**: запрос «Покажи все книги» → вызов tool `get_books` → `GET /api/v1/books`.

**Фрагмент JSON-лога** (из реального запуска `./run.sh "Покажи все книги"`):
```json
{"asctime": "2026-05-20 23:52:17,297", "levelname": "INFO", "name": "tools.get_books", "message": "Tool called", "tool": "get_books", "params": {}}
{"asctime": "2026-05-20 23:52:17,668", "levelname": "INFO", "name": "httpx", "message": "HTTP Request: GET http://books-catalog:5000/api/v1/books \"HTTP/1.1 200 OK\""}
```

## 5. Тестовые запросы

### Запрос 1: Покажи все книги

```
./run.sh "Покажи все книги"
```

**Ответ агента:**
```
Status: success
Action: Получен список всех книг из каталога
Data:
1. Мастер и Маргарита, Михаил Афанасьевич Булгаков, 1967 г., ISBN: 978-5-17-090000-3, издательство: АСТ, страна: Россия
2. Вишнёвый сад, Антон Павлович Чехов, 1904 г., ISBN: 978-5-17-090000-9, издательство: Азбука, страна: Россия
3. Идиот, Фёдор Михайлович Достоевский, 1869 г., ISBN: 978-5-17-090000-7, издательство: АСТ, страна: Россия
4. Преступление и наказание, Фёдор Михайлович Достоевский, 1866 г., ISBN: 978-5-17-090000-2, издательство: АСТ, страна: Россия
5. Тихий Дон, Михаил Александрович Шолохов, 1940 г., ISBN: 978-5-17-090001-0, издательство: АСТ, страна: Россия
6. Анна Каренина, Лев Николаевич Толстой, 1878 г., ISBN: 978-5-17-090000-8, издательство: Эксмо, страна: Россия
7. Мёртвые души, Николай Васильевич Гоголь, 1842 г., ISBN: 978-5-17-090000-5, издательство: Эксмо, страна: Россия
8. Война и мир, Лев Николаевич Толстой, 1869 г., ISBN: 978-5-17-090000-1, издательство: АСТ, страна: Россия
9. Евгений Онегин, Александр Сергеевич Пушкин, 1833 г., ISBN: 978-5-17-090000-4, издательство: Эксмо, страна: Россия
10. Отцы и дети, Иван Сергеевич Тургенев, 1862 г., ISBN: 978-5-17-090000-6, издательство: Азбука, страна: Россия

Errors: нет
```

**Фрагмент JSON-лога:**
```json
{"asctime": "2026-05-20 23:52:17,297", "levelname": "INFO", "name": "tools.get_books", "message": "Tool called", "tool": "get_books", "params": {}}
```

---

### Запрос 2: Покажи книгу с ID

```
./run.sh "Покажи книгу с ID 1eee6587-d383-4ba1-b1da-5bad1e0c828e"
```

**Ответ агента:**
```
Status: success
Action: Получение книги по ID
Data: {"id":"1eee6587-d383-4ba1-b1da-5bad1e0c828e","title":"Мастер и Маргарита","authors":["Михаил Афанасьевич Булгаков"],"isbn":"978-5-17-090000-3","publisher":"АСТ","year":1967,"country":"Россия"}
Errors: нет
```

**Фрагмент JSON-лога:**
```json
{"asctime": "2026-05-20 23:57:43,331", "levelname": "INFO", "name": "tools.get_book_by_id", "message": "Tool called", "tool": "get_book_by_id", "params": {"book_id": "1eee6587-d383-4ba1-b1da-5bad1e0c828e"}}
```

---

### Запрос 3: Создай книгу

```
./run.sh "Создай книгу с названием 'Чистый код' автора Роберт Мартин"
```

**Ответ агента:**
```
Status: success
Action: Создана новая книга в каталоге
Data: {"id":"02a77aa6-7395-46a7-9929-b02214d54529","title":"Чистый код","authors":["Роберт Мартин"],"isbn":null,"publisher":null,"year":null,"country":null}
Errors: нет
```

**Фрагмент JSON-лога:**
```json
{"asctime": "2026-05-21 00:00:28,822", "levelname": "INFO", "name": "tools.create_book", "message": "Tool called", "tool": "create_book", "params": {"title": "Чистый код", "authors": ["Роберт Мартин"], "isbn": null, "publisher": null, "year": null, "country": null}}
```

---

### Запрос 4: Обнови название книги

```
./run.sh "Обнови название книги с ID 1eee6587-d383-4ba1-b1da-5bad1e0c828e на 'Clean Code'"
```

**Ответ агента:**
```
Status: success
Action: Обновление книги по ID 1eee6587-d383-4ba1-b1da-5bad1e0c828e на 'Clean Code'
Data: {
  "id": "1eee6587-d383-4ba1-b1da-5bad1e0c828e",
  "title": "Clean Code",
  "authors": [],
  "isbn": null,
  "publisher": null,
  "year": null,
  "country": null
}
Errors: нет
```

**Фрагмент JSON-лога:**
```json
{"asctime": "2026-05-21 00:02:58,098", "levelname": "INFO", "name": "tools.update_book", "message": "Tool called", "tool": "update_book", "params": {"book_id": "1eee6587-d383-4ba1-b1da-5bad1e0c828e", "title": "Clean Code", "authors": null, "isbn": null, "publisher": null, "year": null, "country": null}}
```

---

### Запрос 5: Удали книгу

```
./run.sh "Удали книгу с ID 02a77aa6-7395-46a7-9929-b02214d54529"
```

**Ответ агента:**
```
Status: success
Action: Удаление книги из каталога по её уникальному идентификатору
Data: {"status": "deleted", "book_id": "02a77aa6-7395-46a7-9929-b02214d54529"}
Errors: нет
```

**Фрагмент JSON-лога:**
```json
{"asctime": "2026-05-21 00:05:22,898", "levelname": "INFO", "name": "tools.delete_book", "message": "Tool called", "tool": "delete_book", "params": {"book_id": "02a77aa6-7395-46a7-9929-b02214d54529"}}
```

---

## 6. Использованные промпты

- [`homework/prompts/00-init-repository.md`](prompts/00-init-repository.md) — инициализация репозитория
- [`homework/prompts/01-backend-adr.md`](prompts/01-backend-adr.md) — ADR для backend
- [`homework/prompts/02-backend-api.md`](prompts/02-backend-api.md) — реализация backend API
- [`homework/prompts/03-agent-adr.md`](prompts/03-agent-adr.md) — ADR для агента
- [`homework/prompts/04-agent-implementation.md`](prompts/04-agent-implementation.md) — реализация агента
- [`homework/prompts/05-scripts.md`](prompts/05-scripts.md) — скрипты подготовки и запуска
- [`homework/prompts/06-report.md`](prompts/06-report.md) — отчёт о выполненной работе
- [`apps/agent/prompts/system.md`](../apps/agent/prompts/system.md) — системный промт агента
