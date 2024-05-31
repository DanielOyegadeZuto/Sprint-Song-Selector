FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Song-Selector-app/Song-Selector-app.csproj", "Song-Selector-app/"]
RUN dotnet restore "Song-Selector-app/Song-Selector-app.csproj"
COPY . .
WORKDIR "/src/Song-Selector-app"
RUN dotnet build "Song-Selector-app.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Song-Selector-app.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Song-Selector-app.dll"]
