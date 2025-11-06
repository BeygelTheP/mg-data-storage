# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/MG.DataStorage.API/MG.DataStorage.API.csproj", "MG.DataStorage.API/"]
COPY ["src/MG.DataStorage.Core/MG.DataStorage.Core.csproj", "MG.DataStorage.Core/"]
COPY ["src/MG.DataStorage.Business/MG.DataStorage.Business.csproj", "MG.DataStorage.Business/"]
COPY ["src/MG.DataStorage.Infrastructure/MG.DataStorage.Infrastructure.csproj", "MG.DataStorage.Infrastructure/"]

# Restore
RUN dotnet restore "MG.DataStorage.API/MG.DataStorage.API.csproj"

# Copy source
COPY src/ .

# Build
WORKDIR "/src/MG.DataStorage.API"
RUN dotnet build "MG.DataStorage.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "MG.DataStorage.API.csproj" -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Create data directory
RUN mkdir -p /app/data

COPY --from=publish /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "MG.DataStorage.API.dll"]