FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["OnlineStore.Gateway/OnlineStore.Gateway.csproj", "OnlineStore.Gateway/"]
RUN dotnet restore "OnlineStore.Gateway/OnlineStore.Gateway.csproj"
COPY . .
WORKDIR "/src/OnlineStore.Gateway"
RUN dotnet build "OnlineStore.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineStore.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineStore.Gateway.dll"]