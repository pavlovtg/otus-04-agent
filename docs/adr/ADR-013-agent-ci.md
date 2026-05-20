# ADR-013: CI для агента

- **Контекст**: необходимо настроить CI для AI-агента независимо от backend.
- **Решение**: отдельный GitHub Actions workflow с одним job (test → coverage check ≥ 90%), триггеры `push` и `pull_request`; ошибка любого шага блокирует PR.
