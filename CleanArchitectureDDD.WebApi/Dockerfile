FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ARG BUILD_CONFIGURATION=Debug
COPY ["CleanArchitectureDDD.WebApi/CleanArchitectureDDD.WebApi.csproj", "CleanArchitectureDDD.WebApi/"]
COPY ["CleanArchitectureDDD.Application/CleanArchitectureDDD.Application.csproj", "CleanArchitectureDDD.Application/"]
COPY ["CleanArchitectureDDD.Domain/CleanArchitectureDDD.Domain.csproj", "CleanArchitectureDDD.Domain/"]
COPY ["CleanArchitectureDDD.Infrastructure/CleanArchitectureDDD.Infrastructure.csproj", "CleanArchitectureDDD.Infrastructure/"]
RUN dotnet restore "CleanArchitectureDDD.WebApi/CleanArchitectureDDD.WebApi.csproj"
COPY . .
WORKDIR "/src/CleanArchitectureDDD.WebApi"
RUN dotnet build "CleanArchitectureDDD.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "CleanArchitectureDDD.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitectureDDD.WebApi.dll"]
