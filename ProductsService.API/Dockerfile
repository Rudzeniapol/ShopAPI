FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProductsService.API/ProductsService.API.csproj", "ProductsService.API/"]
COPY ["ProductsService.Application/ProductsService.Application.csproj", "ProductsService.Application/"]
COPY ["ProductsService.Domain/ProductsService.Domain.csproj", "ProductsService.Domain/"]
COPY ["ProductsService.Persistence/ProductsService.Persistence.csproj", "ProductsService.Persistence/"]
RUN dotnet restore "ProductsService.API/ProductsService.API.csproj"
COPY . .
WORKDIR "/src/ProductsService.API"
RUN dotnet build "ProductsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProductsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductsService.API.dll"]
