# otus-04-agent

Минимальный агент на LangChain с backend-сервисом управления книгами.

## Требования

- [Docker](https://www.docker.com/) и Docker Compose
- [Ollama](https://ollama.com/) — запущена локально на хосте

## Запуск

1. Запустить Ollama и загрузить модель:

```bash
ollama pull qwen3.5:4b
ollama serve
```

2. Запустить сервисы:

```bash
cd infrastructure/docker-compose
docker compose up --build
```

Сервис запустится на <http://localhost:5000>.

Swagger UI: <http://localhost:5000/api/swagger>.

## API

Базовый URL: `http://localhost:5000/api/v1/books`

- `GET /api/v1/books` — список книг
- `GET /api/v1/books/{id}` — книга по ID
- `POST /api/v1/books` — создать книгу
- `PUT /api/v1/books/{id}` — обновить книгу
- `DELETE /api/v1/books/{id}` — удалить книгу

## Агент

### Настройка

Скопировать `.env.example` и заполнить переменные:

```bash
cd apps/agent
cp .env.example .env
```

Переменные в `.env`:

- `BACKEND_URL` — URL backend-сервиса (например, `http://localhost:5000`)
- `OLLAMA_BASE_URL` — URL Ollama (например, `http://localhost:11434`)

### Запуск через Docker Compose

```bash
cd infrastructure/docker-compose
docker compose run --rm agent "запрос пользователя"
```

### Запуск локально

```bash
cd apps/agent
python main.py "<запрос>"
```

### Примеры запросов

```bash
# Получить список всех книг
docker compose run --rm agent "Покажи все книги в каталоге"

# Создать новую книгу
docker compose run --rm agent "Добавь книгу 'Мастер и Маргарита' автора Булгаков, год 1967"

# Удалить книгу по ID
docker compose run --rm agent "Удали книгу с ID 123e4567-e89b-12d3-a456-426614174000"
```
