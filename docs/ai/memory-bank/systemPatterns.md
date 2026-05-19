# System Patterns

- Агент: LangChain ReAct или Tool-calling агент.
- Tool: оформлен как `@tool` декоратор LangChain, внутри — HTTP-вызов.
- Системный промт: роль агента, ограничения, правила вызова tool.
- Контракт ответа: фиксированный текстовый формат (Status / Action / Data / Errors).
- Структура проекта: `main.py` (точка входа), `tools/` (API-tools), `prompts/` (системные промты).
