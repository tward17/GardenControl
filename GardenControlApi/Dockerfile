#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001
ENV ASPNETCORE_URLS="http://*:5000;http://*:5001"
ENV DATABASE_CONNECTIONSTRING=

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GardenControlApi/GardenControlApi.csproj", "GardenControlApi/"]
COPY ["GardenControlRepositories/GardenControlRepositories.csproj", "GardenControlRepositories/"]
COPY ["GardenControlCore/GardenControlCore.csproj", "GardenControlCore/"]
COPY ["GardenControlServices/GardenControlServices.csproj", "GardenControlServices/"]
RUN dotnet restore "GardenControlApi/GardenControlApi.csproj"
COPY . .
WORKDIR "/src/GardenControlApi"
RUN dotnet build "GardenControlApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GardenControlApi.csproj" -c Release -o /app/publish -r linux-arm

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim-arm32v7

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GardenControlApi.dll"]
