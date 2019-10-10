FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app

EXPOSE 8081

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /project

COPY VN.Example.sln ./
COPY src/VN.Example.Host.Web/VN.Example.Host.Web.csproj src/VN.Example.Host.Web/
COPY src/VN.Example.Infrastructure/VN.Example.Infrastructure.csproj src/VN.Example.Infrastructure/
COPY src/VN.Example.Platform/VN.Example.Platform.csproj src/VN.Example.Platform/
COPY src/VN.Example.Infrastructure.Provider.MessageBus/VN.Example.Infrastructure.Provider.MessageBus.csproj src/VN.Example.Infrastructure.Provider.MessageBus/
COPY src/VN.Example.Infrastructure.Database.MSSQL/VN.Example.Infrastructure.Database.MSSQL.csproj src/VN.Example.Infrastructure.Database.MSSQL/
COPY src/VN.Example.Infrastructure.Database.Couchbase/VN.Example.Infrastructure.Database.Couchbase.csproj src/VN.Example.Infrastructure.Database.Couchbase/
#COPY docker-compose.dcproj ./

RUN dotnet restore VN.Example.sln

COPY . .
WORKDIR /project/

RUN dotnet build VN.Example.sln -c Release -o /app/build --no-restore

FROM build AS publish

RUN dotnet publish "src/VN.Example.Host.Web/VN.Example.Host.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VN.Example.Host.Web.dll"]
