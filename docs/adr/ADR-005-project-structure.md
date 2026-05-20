# ADR-002: Структура папок репозитория

- **Контекст**: необходимо зафиксировать единую структуру репозитория для всех участников и AI-агента.
- **Решение**: использовать следующую структуру:
  - `/apps` — приложения и домены.
  - `/apps/backend/` — домен backend-сервиса.
  - `/apps/backend/Backend.slnx` — solution backend.
  - `/apps/backend/src/` — исходный код backend-сервиса.
  - `/apps/backend/tests/` — тесты backend-сервиса.
  - `/apps/agent/` — домен AI-агента.
  - `/infrastructure/docker-compose/` — файлы для развёртывания через docker-compose.
  - `/docs/contracts/openapi/` — OpenAPI-спецификации, разделённые по доменам и сервисам.
