# ADR-015: Интеграция с API backend через LangChain Tools

- **Контекст**: необходимо определить способ интеграции агента с HTTP API backend-сервиса.
- **Решение**: каждая операция API backend (get/create/update/delete) реализуется отдельным LangChain Tool.
