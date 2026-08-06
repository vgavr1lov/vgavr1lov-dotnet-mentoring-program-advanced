FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["ECommerce.Catalog/WebAPI/ECommerce.Catalog.WebAPI.csproj", "ECommerce.Catalog/WebAPI/"]
COPY ["ECommerce.Catalog/Infrastructure/ECommerce.Catalog.Infrastructure.csproj", "ECommerce.Catalog/Infrastructure/"]
COPY ["ECommerce.Catalog/Application/ECommerce.Catalog.Application.csproj", "ECommerce.Catalog/Application/"]
COPY ["ECommerce.Catalog/Domain/ECommerce.Catalog.Domain.csproj", "ECommerce.Catalog/Domain/"]
COPY ["ECommerce.Messaging.RabbitMq/ECommerce.Messaging.RabbitMq.csproj", "ECommerce.Messaging.RabbitMq/"]
COPY ["ECommerce.Common.Contracts/ECommerce.Common.Contracts.csproj", "ECommerce.Common.Contracts/"]
COPY ["ECommerce.Common.Abstractions/ECommerce.Common.Abstractions.csproj", "ECommerce.Common.Abstractions/"]

RUN dotnet restore "ECommerce.Catalog/WebAPI/ECommerce.Catalog.WebAPI.csproj"

COPY . .

WORKDIR "/src/ECommerce.Catalog/WebAPI"

RUN dotnet publish "ECommerce.Catalog.WebAPI.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Container

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ECommerce.Catalog.WebAPI.dll"]