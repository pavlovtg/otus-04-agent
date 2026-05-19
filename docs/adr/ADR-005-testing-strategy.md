# ADR-005: Стратегия тестирования backend-сервиса

- **Контекст**: необходимо определить виды тестов и минимальный порог покрытия кода.
- **Решение**: покрывать код unit-тестами (domain/application), integration-тестами (HTTP через `WebApplicationFactory`) и microservice-тестами (`WebApplicationFactory` без моков внешних зависимостей); минимальный code coverage — 90%, CI падает при нарушении.
- **Фреймворк**: xUnit.
- **Нейминг**: тестовый проект называется `<ProjectName>.Tests.csproj`, где `<ProjectName>` — название тестируемого проекта.
