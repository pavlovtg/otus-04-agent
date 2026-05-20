# Скрипты подготовки и запуска агента

## Роль

- Ты — Senior DevOps-инженер, пишущий bash-скрипты для автоматизации запуска проекта.

## Контекст

- Проект: AI-агент на LangChain + backend BooksCatalog, запускаются через Docker Compose.
- Docker Compose файл: `infrastructure/docker-compose/docker-compose.yml`.
- Конфигурация агента: `apps/agent/.env` (копируется из `apps/agent/.env.example`).
- LLM: локальная Ollama, модель `qwen3.5:4b`.
- Все скрипты выполняются из корня репозитория.

## Задача

1. Создать два bash-скрипта в корне репозитория: `setup.sh` и `run.sh`.
2. Дополнить корневой `README.md`.

## Требования

### `setup.sh`

- Скачать модель Ollama: `ollama pull qwen3.5:4b`.
- Если Ollama не запущена (не отвечает на `http://localhost:11434`), запустить `ollama serve` в фоне и дождаться готовности.
- Скопировать `apps/agent/.env.example` → `apps/agent/.env`, только если `apps/agent/.env` не существует.
- Собрать Docker Compose: `docker compose -f infrastructure/docker-compose/docker-compose.yml build`.
- При ошибке любого шага — вывести сообщение и завершить с кодом 1.
- Вывести итоговое сообщение об успешной подготовке.

### `run.sh`

- Принять запрос пользователя первым аргументом `$1`.
- Если аргумент не задан или пустой — вывести сообщение об ошибке в stderr и завершить с кодом 1.
- Запустить агента: `docker compose -f infrastructure/docker-compose/docker-compose.yml run --rm agent "$1"`.

### Общие требования к скриптам

- Shebang: `#!/usr/bin/env bash`.
- `set -euo pipefail` в начале каждого скрипта.
- Скрипты должны быть исполняемыми (`chmod +x`).
- Комментарии на русском языке.

### `README.md`

- Добавить раздел «Быстрый старт» в начало файла (перед разделом «Требования»).
- Указать, что это рекомендуемый способ запуска.
- Содержимое раздела:

  ```bash
  ./setup.sh          # подготовка окружения (один раз)
  ./run.sh "<запрос>" # запуск агента
  ```

- Остальные разделы README оставить без изменений.
