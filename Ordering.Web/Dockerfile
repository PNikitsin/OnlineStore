FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5006

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Ordering.Web/Ordering.Web.csproj", "Ordering.Web/"]
COPY ["OnlineStore.Shared/OnlineStore.Shared.csproj", "OnlineStore.Shared/"]
COPY ["Ordering.Application/Ordering.Application.csproj", "Ordering.Application/"]
COPY ["Ordering.Domain/Ordering.Domain.csproj", "Ordering.Domain/"]
COPY ["Ordering.Infrastructure/Ordering.Infrastructure.csproj", "Ordering.Infrastructure/"]
RUN dotnet restore "Ordering.Web/Ordering.Web.csproj"
COPY . .
WORKDIR "/src/Ordering.Web"
RUN dotnet build "Ordering.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.Web.dll"]