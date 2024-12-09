#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Udemy.Auth/Udemy.Auth.API/Udemy.Auth.API.csproj", "Udemy.Auth/Udemy.Auth.API/"]
COPY ["Udemy.Auth/Udemy.Auth.Application/Udemy.Auth.Application.csproj", "Udemy.Auth/Udemy.Auth.Application/"]
COPY ["Udemy.Auth/Udemy.Auth.Domain/Udemy.Auth.Domain.csproj", "Udemy.Auth/Udemy.Auth.Domain/"]
COPY ["Udemy.Auth/Udemy.Auth.Contracts/Udemy.Auth.Contracts.csproj", "Udemy.Auth/Udemy.Auth.Contracts/"]
COPY ["Udemy.Auth/Udemy.Auth.Infrastructure/Udemy.Auth.Infrastructure.csproj", "Udemy.Auth/Udemy.Auth.Infrastructure/"]
COPY ["Udemy.Common/Udemy.Common/Udemy.Common.csproj", "Udemy.Common/Udemy.Common/"]
RUN dotnet restore "./Udemy.Auth/Udemy.Auth.API/Udemy.Auth.API.csproj"
COPY . .
WORKDIR "/src/Udemy.Auth/Udemy.Auth.API"
RUN dotnet build "./Udemy.Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Udemy.Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Udemy.Auth.API.dll"]