FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["ECommerce.Identity/ECommerce.Identity.csproj", "ECommerce.Identity/"]

RUN dotnet restore "ECommerce.Identity/ECommerce.Identity.csproj"

COPY . .

WORKDIR "/src/ECommerce.Identity"

RUN dotnet publish "ECommerce.Identity.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Container

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ECommerce.Identity.dll"]