FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["OnlineStore.Client/OnlineStore.Client.csproj", "OnlineStore.Client/"]
RUN dotnet restore "OnlineStore.Client/OnlineStore.Client.csproj"
COPY . .
WORKDIR "/src/OnlineStore.Client"
RUN dotnet build "OnlineStore.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineStore.Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineStore.Client.dll"]