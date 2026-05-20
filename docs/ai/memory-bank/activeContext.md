# Active Context

## Текущий этап

Реализован Python AI-агент в `/apps/agent/`.

## Что сделано в этой сессии

- Убран сервис `ollama` из `docker-compose.yml`; Ollama теперь запускается на хосте
- `OLLAMA_BASE_URL` в docker-compose изменён на `http://host.docker.internal:11434`
- Обновлён `ADR-016`: Ollama вне docker-compose
- Обновлён `README.md`: добавлен шаг запуска Ollama на хосте
- Обновлены `homework/prompts/03-agent-adr.md` и `04-agent-implementation.md`

## Следующие шаги

- Нет

## Открытые вопросы

- Нет
