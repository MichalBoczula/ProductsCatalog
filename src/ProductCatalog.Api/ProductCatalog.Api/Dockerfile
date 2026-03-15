# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/ProductCatalog.Api/ProductCatalog.Api/ProductCatalog.Api.csproj", "ProductCatalog.Api/ProductCatalog.Api/"]
COPY ["src/ProductCatalog.Application/ProductCatalog.Application.csproj", "ProductCatalog.Application/"]
COPY ["src/ProductCatalog.Domain/ProductCatalog.Domain.csproj", "ProductCatalog.Domain/"]
COPY ["src/ProductCatalog.Infrastructure/ProductCatalog.Infrastructure.csproj", "ProductCatalog.Infrastructure/"]

RUN dotnet restore "./ProductCatalog.Api/ProductCatalog.Api/ProductCatalog.Api.csproj"

COPY . .

WORKDIR "/src/src/ProductCatalog.Api/ProductCatalog.Api"
RUN dotnet build "./ProductCatalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProductCatalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductCatalog.Api.dll"]