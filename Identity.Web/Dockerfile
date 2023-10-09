FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Identity.Web/Identity.Web.csproj", "Identity.Web/"]
COPY ["Identity.Application/Identity.Application.csproj", "Identity.Application/"]
COPY ["Identity.Domain/Identity.Domain.csproj", "Identity.Domain/"]
COPY ["Identity.Infrastructure/Identity.Infrastructure.csproj", "Identity.Infrastructure/"]
COPY ["OnlineStore.Shared/OnlineStore.Shared.csproj", "OnlineStore.Shared/"]
RUN dotnet restore "Identity.Web/Identity.Web.csproj"
COPY . .
WORKDIR "/src/Identity.Web"
RUN dotnet build "Identity.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Web.dll"]