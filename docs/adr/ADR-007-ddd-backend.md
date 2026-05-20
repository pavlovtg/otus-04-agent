# ADR-004: Domain Driven Design для backend-сервиса

- **Контекст**: необходимо выбрать архитектурный подход для организации кода backend-сервиса.
- **Решение**: использовать DDD с разделением на слои внутри одного .csproj: `Domain/`, `Application/`, `Infrastructure/`, `Adapters/`.
