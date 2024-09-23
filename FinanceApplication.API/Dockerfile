FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FinanceApplication.API/FinanceApplication.API.csproj", "FinanceApplication.API/"]
COPY ["FinanceApplication.Business/FinanceApplication.Business.csproj", "FinanceApplication.Business/"]
COPY ["FinanceApplication.Core/FinanceApplication.Core.csproj", "FinanceApplication.Core/"]
COPY ["FinanceApplication.Entities/FinanceApplication.Entities.csproj", "FinanceApplication.Entities/"]
COPY ["FinanceApplication.Dal/FinanceApplication.Dal.csproj", "FinanceApplication.Dal/"]
RUN dotnet restore "FinanceApplication.API/FinanceApplication.API.csproj"
COPY . .
WORKDIR "/src/FinanceApplication.API"
RUN dotnet build "FinanceApplication.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FinanceApplication.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceApplication.API.dll"]
