FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["ZombieDefenseSystem.Api/ZombieDefenseSystem.Api.csproj", "ZombieDefenseSystem.Api/"]
COPY ["ZombieDefenseSystem.Application/ZombieDefenseSystem.Application.csproj", "ZombieDefenseSystem.Application/"]
COPY ["ZombieDefenseSystem.Domain/ZombieDefenseSystem.Domain.csproj", "ZombieDefenseSystem.Domain/"]
COPY ["ZombieDefenseSystem.Infrastructure/ZombieDefenseSystem.Infrastructure.csproj", "ZombieDefenseSystem.Infrastructure/"]

RUN dotnet restore "./ZombieDefenseSystem.Api/ZombieDefenseSystem.Api.csproj"

COPY . .
WORKDIR "/src/ZombieDefenseSystem.Api"
RUN dotnet publish "./ZombieDefenseSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "ZombieDefenseSystem.Api.dll"]