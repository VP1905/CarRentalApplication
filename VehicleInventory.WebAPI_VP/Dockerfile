# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["VehicleInventory.WebAPI_VP/VehicleInventory.WebAPI_VP.csproj", "VehicleInventory.WebAPI_VP/"]
COPY ["VehicleInventory.Application_VP/VehicleInventory.Application_VP.csproj", "VehicleInventory.Application_VP/"]
COPY ["VehicleInventory.Domain_VP/VehicleInventory.Domain_VP.csproj", "VehicleInventory.Domain_VP/"]
COPY ["VehicleInventory.Infrastructure_VP/VehicleInventory.Infrastructure_VP.csproj", "VehicleInventory.Infrastructure_VP/"]
RUN dotnet restore "./VehicleInventory.WebAPI_VP/VehicleInventory.WebAPI_VP.csproj"
COPY . .
WORKDIR "/src/VehicleInventory.WebAPI_VP"
RUN dotnet build "./VehicleInventory.WebAPI_VP.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VehicleInventory.WebAPI_VP.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VehicleInventory.WebAPI_VP.dll"]