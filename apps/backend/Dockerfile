FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/BooksCatalog/BooksCatalog.csproj src/BooksCatalog/
RUN dotnet restore src/BooksCatalog/BooksCatalog.csproj

COPY src/BooksCatalog/ src/BooksCatalog/
RUN dotnet publish src/BooksCatalog/BooksCatalog.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
COPY src/BooksCatalog/books.json .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "BooksCatalog.dll"]
