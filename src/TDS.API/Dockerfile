#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TDS.API/TDS.API.csproj", "TDS.API/"]
COPY ["TDS.Infrastructure/TDS.Infrastructure.csproj", "TDS.Infrastructure/"]
COPY ["TDS.Domain/TDS.Domain.csproj", "TDS.Domain/"]
RUN dotnet restore "TDS.API/TDS.API.csproj"
COPY . .
WORKDIR "/src/TDS.API"
RUN dotnet build "TDS.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TDS.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TDS.API.dll"]