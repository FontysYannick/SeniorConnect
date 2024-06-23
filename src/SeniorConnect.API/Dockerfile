# Stage 1: Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8081

# Stage 2: Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SeniorConnect.API/SeniorConnect.API.csproj", "SeniorConnect.API/"]
RUN dotnet restore "./SeniorConnect.API/SeniorConnect.API.csproj"
COPY . .
WORKDIR "/src/SeniorConnect.API"
RUN dotnet build "./SeniorConnect.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SeniorConnect.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeniorConnect.API.dll"]
