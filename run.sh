#!/usr/bin/env bash
set -euo pipefail

# Проверить, что запрос передан первым аргументом
if [ -z "${1:-}" ]; then
  echo "Ошибка: укажите запрос первым аргументом." >&2
  echo "Использование: ./run.sh \"<запрос>\"" >&2
  exit 1
fi

# Запустить агента
docker compose -f infrastructure/docker-compose/docker-compose.yml run --rm agent "$1"
