FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY WebApi.sln .
COPY WebApi.Domain/WebApi.Domain.csproj WebApi.Domain/
COPY WebApi.Repo/WebApi.Repo.csproj     WebApi.Repo/
COPY WebApi/WebApi.API.csproj           WebApi/

RUN dotnet restore

COPY . .
RUN dotnet publish WebApi/WebApi.API.csproj \
    -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WebApi.API.dll"]
