# otus-04-agent

Минимальный агент на LangChain с backend-сервисом управления книгами.

## Требования

- [Docker](https://www.docker.com/) и Docker Compose

## Запуск

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
