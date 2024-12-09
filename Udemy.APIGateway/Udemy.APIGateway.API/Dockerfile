#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Udemy.APIGateway/Udemy.APIGateway.API/Udemy.APIGateway.API.csproj", "Udemy.APIGateway/Udemy.APIGateway.API/"]
COPY ["Udemy.Auth/Udemy.Auth.Contracts/Udemy.Auth.Contracts.csproj", "Udemy.Auth/Udemy.Auth.Contracts/"]
COPY ["Udemy.Common/Udemy.Common/Udemy.Common.csproj", "Udemy.Common/Udemy.Common/"]
RUN dotnet restore "./Udemy.APIGateway/Udemy.APIGateway.API/Udemy.APIGateway.API.csproj"
COPY . .
WORKDIR "/src/Udemy.APIGateway/Udemy.APIGateway.API"
RUN dotnet build "./Udemy.APIGateway.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Udemy.APIGateway.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Udemy.APIGateway.API.dll"]