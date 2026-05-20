# ADR-016: Ollama вне docker-compose

- **Контекст**: необходимо определить способ запуска Ollama в инфраструктуре проекта.
- **Решение**: Ollama запускается на хосте (не в docker-compose); URL передаётся агенту через переменную окружения `OLLAMA_BASE_URL` (в docker-compose — `http://host.docker.internal:11434`).
