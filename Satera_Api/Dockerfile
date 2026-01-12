# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base runtime image (use .NET 9 to match the project TargetFramework)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Build image (use .NET SDK 9)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Satera_Api/Satera_Api.csproj", "Satera_Api/"]
RUN dotnet restore "Satera_Api/Satera_Api.csproj"
COPY . .
WORKDIR "/src/Satera_Api"
RUN dotnet build "Satera_Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish into a self-contained output folder (static files set to CopyToOutputDirectory in the csproj will be included)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Satera_Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
ARG APP_UID=1000
WORKDIR /app
COPY --from=publish /app/publish .

# Make files readable/executable and set ownership so a non-root runtime user works consistently.
# (chown/chmod are best-effort and won't fail the build if they are not applicable)
RUN chmod -R a+rX /app || true
RUN chown -R ${APP_UID}:${APP_UID} /app || true

USER ${APP_UID}
ENTRYPOINT ["dotnet", "Satera_Api.dll"]