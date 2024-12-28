FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Adjust the COPY commands based on the new Dockerfile location
COPY ["Upload.API.csproj", "."]
COPY ["D:/workspace/BE/microservices/ecommerce/src/BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["D:/workspace/BE/microservices/ecommerce/src/BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]

# Restore dependencies
RUN dotnet restore "Upload/Upload.API/Upload.API.csproj"

# Copy the rest of the code
COPY . .

# Build and publish the app
WORKDIR "/src/Upload/Upload.API"
RUN dotnet build "Upload.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Upload.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Upload.API.dll"]
