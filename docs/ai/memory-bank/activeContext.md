# Active Context

## Текущий этап

Зафиксированы архитектурные решения для AI-агента (ADR-010..016), обновлён ADR-005.

## Что сделано в этой сессии

- Обновлён ADR-005: добавлен `/apps/agent/` — домен AI-агента
- Созданы ADR-010..016:
  - ADR-010: Python для AI-агента
  - ADR-011: LangChain как фреймворк агента
  - ADR-012: Стратегия тестирования агента (pytest, coverage ≥ 90%)
  - ADR-013: CI для агента (отдельный GitHub Actions workflow)
  - ADR-014: LLM-провайдер — Ollama + qwen3.5:4b
  - ADR-015: Интеграция с API backend через LangChain Tools
  - ADR-016: Ollama в docker-compose
- Обновлён индекс `docs/adr/README.md`

## Следующие шаги

- Реализовать Python AI-агент на LangChain в `/apps/agent/`

## Открытые вопросы

- Нет
