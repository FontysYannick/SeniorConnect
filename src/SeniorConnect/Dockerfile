# Stage 1: Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8081

# Stage 2: Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SeniorConnect/SeniorConnect.csproj", "SeniorConnect/"]
RUN dotnet restore "SeniorConnect/SeniorConnect.csproj"
COPY . .
WORKDIR "/src/SeniorConnect"
RUN dotnet build "SeniorConnect.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SeniorConnect.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeniorConnect.dll"]
