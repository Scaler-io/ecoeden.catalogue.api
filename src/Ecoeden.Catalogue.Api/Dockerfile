#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

COPY src/Ecoeden.Catalogue.Domain/Ecoeden.Catalogue.Domain.csproj, src/Ecoeden.Catalogue.Domain
COPY src/Ecoeden.Swagger/Ecoeden.Swagger.csproj, src/Ecoeden.Swagger
COPY src/Ecoeden.Catalogue.Application/Ecoeden.Catalogue.Application.csproj, src/Ecoeden.Catalogue.Application
COPY src/Ecoeden.Catalogue.Infrastructure/Ecoeden.Catalogue.Infrastructure.csproj, src/Ecoeden.Catalogue.Infrastructure
COPY src/Ecoeden.Catalogue.Api/Ecoeden.Catalogue.Api.csproj, src/Ecoeden.Catalogue.Api

RUN dotnet restore "src/Ecoeden.Catalogue.Api/Ecoeden.Catalogue.Api.csproj"

COPY . .
WORKDIR "/src/Ecoeden.Catalogue.Api"
RUN dotnet build "./Ecoeden.Catalogue.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ecoeden.Catalogue.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecoeden.Catalogue.Api.dll"]