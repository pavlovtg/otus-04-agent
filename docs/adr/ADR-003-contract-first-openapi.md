# ADR-003: Contract-first с OpenAPI v3

- **Контекст**: необходимо выбрать подход к проектированию API между code-first и contract-first.
- **Решение**: использовать contract-first — сначала создаётся OpenAPI v3 спецификация в формате YAML, затем реализуется сервис; спецификации хранятся в `/docs/contracts/openapi/`.
