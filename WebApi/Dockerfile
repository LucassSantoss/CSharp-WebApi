FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1.1) Copia a solução e projetos para cache de restore
COPY WebApi.sln .
COPY WebApi.Domain/WebApi.Domain.csproj WebApi.Domain/
COPY WebApi.Repo/WebApi.Repo.csproj     WebApi.Repo/
COPY WebApi/WebApi.API.csproj           WebApi/

# 1.2) Roda o restore
RUN dotnet restore

# 1.3) Copia todo o código e publica
COPY . .
RUN dotnet publish WebApi/WebApi.API.csproj \
    -c Release -o /app/publish

# ---------------------------------------------------
# 2) Runtime (somente ASP.NET Runtime)
# ---------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copia o output do build
COPY --from=build /app/publish .

# Inicia sua API
ENTRYPOINT ["dotnet", "WebApi.API.dll"]
