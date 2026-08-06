FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["ECommerce.Cart/WebAPI/ECommerce.Cart.WebAPI.csproj", "ECommerce.Cart/WebAPI/"]
COPY ["ECommerce.Cart/Infrastructure/ECommerce.Cart.Infrastructure.csproj", "ECommerce.Cart/Infrastructure/"]
COPY ["ECommerce.Cart/Application/ECommerce.Cart.Application.csproj", "ECommerce.Cart/Application/"]
COPY ["ECommerce.Cart/Domain/ECommerce.Cart.Domain.csproj", "ECommerce.Cart/Domain/"]
COPY ["ECommerce.Messaging.RabbitMq/ECommerce.Messaging.RabbitMq.csproj", "ECommerce.Messaging.RabbitMq/"]
COPY ["ECommerce.Common.Contracts/ECommerce.Common.Contracts.csproj", "ECommerce.Common.Contracts/"]
COPY ["ECommerce.Common.Abstractions/ECommerce.Common.Abstractions.csproj", "ECommerce.Common.Abstractions/"]

RUN dotnet restore "ECommerce.Cart/WebAPI/ECommerce.Cart.WebAPI.csproj"

COPY . .

WORKDIR "/src/ECommerce.Cart/WebAPI"

RUN dotnet publish "ECommerce.Cart.WebAPI.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Container

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ECommerce.Cart.WebAPI.dll"]