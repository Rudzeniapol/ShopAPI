FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ClientsService.API/ClientsService.API.csproj", "ClientsService.API/"]
COPY ["ClientsService.Application/ClientsService.Application.csproj", "ClientsService.Application/"]
COPY ["ClientsService.Domain/ClientsService.Domain.csproj", "ClientsService.Domain/"]
COPY ["ClientsService.Persistence/ClientsService.Persistence.csproj", "ClientsService.Persistence/"]
RUN dotnet restore "ClientsService.API/ClientsService.API.csproj"
COPY . .
WORKDIR "/src/ClientsService.API"
RUN dotnet build "ClientsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ClientsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClientsService.API.dll"]
