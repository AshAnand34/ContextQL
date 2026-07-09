FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/ContextQL.Api/ContextQL.Api.csproj", "src/ContextQL.Api/"]
COPY ["src/ContextQL.Domain/ContextQL.Domain.csproj", "src/ContextQL.Domain/"]
COPY ["src/ContextQL.Persistence/ContextQL.Persistence.csproj", "src/ContextQL.Persistence/"]
COPY ["src/ContextQL.Sync/ContextQL.Sync.csproj", "src/ContextQL.Sync/"]

RUN dotnet restore "src/ContextQL.Api/ContextQL.Api.csproj"

COPY . .
WORKDIR "/src/src/ContextQL.Api"
RUN dotnet publish "ContextQL.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "ContextQL.Api.dll"]
