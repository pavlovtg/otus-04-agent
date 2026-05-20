#!/usr/bin/env bash
set -euo pipefail

# URL Ollama (необязательный первый аргумент, по умолчанию http://localhost:11434)
OLLAMA_HOST="${1:-http://localhost:11434}"

# Проверить доступность Ollama
if ! curl -sf "${OLLAMA_HOST}" > /dev/null 2>&1; then
  echo "Ollama не запущена — запускаем ollama serve в фоне..."
  ollama serve &

  # Ждать готовности Ollama (до 30 секунд)
  echo "Ожидаем готовности Ollama..."
  for i in $(seq 1 30); do
    if curl -sf "${OLLAMA_HOST}" > /dev/null 2>&1; then
      echo "Ollama готова."
      break
    fi
    if [ "$i" -eq 30 ]; then
      echo "Ошибка: Ollama не стала доступна за 30 секунд." >&2
      exit 1
    fi
    sleep 1
  done
else
  echo "Ollama уже запущена."
fi

# Скачать модель
echo "Скачиваем модель qwen3.5:4b..."
ollama pull qwen3.5:4b || { echo "Ошибка: не удалось скачать модель." >&2; exit 1; }

# Скопировать .env если не существует
if [ ! -f apps/agent/.env ]; then
  echo "Копируем apps/agent/.env.example → apps/agent/.env..."
  cp apps/agent/.env.example apps/agent/.env || { echo "Ошибка: не удалось скопировать .env." >&2; exit 1; }
else
  echo "apps/agent/.env уже существует, пропускаем."
fi

# Если OLLAMA_HOST задан явно — прописать его в .env
if [ -n "${1:-}" ]; then
  echo "Прописываем OLLAMA_BASE_URL=${OLLAMA_HOST} в apps/agent/.env..."
  sed -i.bak "s|^OLLAMA_BASE_URL=.*|OLLAMA_BASE_URL=${OLLAMA_HOST}|" apps/agent/.env && rm -f apps/agent/.env.bak
fi

# Собрать Docker Compose
echo "Собираем Docker Compose..."
docker compose -f infrastructure/docker-compose/docker-compose.yml build || { echo "Ошибка: не удалось собрать Docker Compose." >&2; exit 1; }

echo ""
echo "✅ Подготовка окружения завершена успешно."
