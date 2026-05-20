# otus-04-agent

## Быстрый старт

> Рекомендуемый способ запуска.

```bash
./setup.sh                        # подготовка окружения (один раз)
./setup.sh "http://host:11434"    # если Ollama запущена на нестандартном адресе
./run.sh "<запрос>"               # запуск агента
```

## Требования

- [Docker](https://www.docker.com/) и Docker Compose
- [Ollama](https://ollama.com/) — запущена локально на хосте

## Настройка

Скопировать `.env.example`:

```bash
cp apps/agent/.env.example apps/agent/.env
```

Переменные в `.env`:

- `BACKEND_URL` — URL backend-сервиса; при запуске через Docker Compose менять не нужно — BooksCatalog поднимается автоматически по адресу `http://books-catalog:5000`
- `OLLAMA_BASE_URL` — URL Ollama; если Ollama запущена локально на стандартном порту, менять не нужно — по умолчанию `http://host.docker.internal:11434`

## Запуск

1. Запустить Ollama и загрузить модель:

```bash
ollama pull qwen3.5:4b
ollama serve
```

2. Запустить сервисы:

```bash
docker compose -f infrastructure/docker-compose/docker-compose.yml up --build
```

3. Запустить агента:

```bash
docker compose -f infrastructure/docker-compose/docker-compose.yml run --rm agent "<запрос>"
```
