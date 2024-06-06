FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MarkupPix.Server.WebApi/MarkupPix.Server.WebApi.csproj", "src/MarkupPix.Server.WebApi/"]
COPY ["src/MarkupPix.Business/MarkupPix.Business.csproj", "src/MarkupPix.Business/"]
COPY ["src/MarkupPix.Data/MarkupPix.Data.csproj", "src/MarkupPix.Data/"]
COPY ["src/MarkupPix.Core/MarkupPix.Core.csproj", "src/MarkupPix.Core/"]
COPY ["src/MarkupPix.Server.ApiClient/MarkupPix.Server.ApiClient.csproj", "src/MarkupPix.Server.ApiClient/"]
RUN dotnet restore "src/MarkupPix.Server.WebApi/MarkupPix.Server.WebApi.csproj"
COPY . .
WORKDIR "/src/src/MarkupPix.Server.WebApi"
RUN dotnet build "MarkupPix.Server.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MarkupPix.Server.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "MarkupPix.Server.WebApi.dll"]