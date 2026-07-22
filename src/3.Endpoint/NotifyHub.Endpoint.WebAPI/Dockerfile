# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/3.Endpoint/NotifyHub.Endpoint.WebAPI/NotifyHub.Endpoint.WebAPI.csproj", "src/3.Endpoint/NotifyHub.Endpoint.WebAPI/"]
COPY ["src/1.Core/NotifyHub.Core.ApplicationService/NotifyHub.Core.ApplicationService.csproj", "src/1.Core/NotifyHub.Core.ApplicationService/"]
COPY ["src/2.Infrastructure/NotifyHub.Infrastructure.Services/NotifyHub.Infrastructure.Services.csproj", "src/2.Infrastructure/NotifyHub.Infrastructure.Services/"]
COPY ["src/2.Infrastructure/Data/NotifyHub.Infrastructure.Data.SqlServer/NotifyHub.Infrastructure.Data.SqlServer.csproj", "src/2.Infrastructure/Data/NotifyHub.Infrastructure.Data.SqlServer/"]
COPY ["src/1.Core/NotifyHub.Core.Contracts/NotifyHub.Core.Contracts.csproj", "src/1.Core/NotifyHub.Core.Contracts/"]
COPY ["src/1.Core/NotifyHub.Core.Domain/NotifyHub.Core.Domain.csproj", "src/1.Core/NotifyHub.Core.Domain/"]
COPY ["src/4.Shared/NotifyHub.Shared.Utility/NotifyHub.Shared.Utility.csproj", "src/4.Shared/NotifyHub.Shared.Utility/"]
COPY ["src/1.Core/NotifyHub.Core.BuildingBlocks/NotifyHub.Core.BuildingBlocks.csproj", "src/1.Core/NotifyHub.Core.BuildingBlocks/"]
RUN dotnet restore "./src/3.Endpoint/NotifyHub.Endpoint.WebAPI/NotifyHub.Endpoint.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/3.Endpoint/NotifyHub.Endpoint.WebAPI"
RUN dotnet build "./NotifyHub.Endpoint.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NotifyHub.Endpoint.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotifyHub.Endpoint.WebAPI.dll"]