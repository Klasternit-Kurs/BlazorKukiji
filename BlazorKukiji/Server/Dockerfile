#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["BlazorKukiji/Server/BlazorKukiji.Server.csproj", "BlazorKukiji/Server/"]
COPY ["BlazorKukiji/Shared/BlazorKukiji.Shared.csproj", "BlazorKukiji/Shared/"]
COPY ["BlazorKukiji/Client/BlazorKukiji.Client.csproj", "BlazorKukiji/Client/"]
RUN dotnet restore "BlazorKukiji/Server/BlazorKukiji.Server.csproj"
COPY . .
WORKDIR "/src/BlazorKukiji/Server"
RUN dotnet build "BlazorKukiji.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorKukiji.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["cert/*", "/root/.aspnet/https/"]
ENTRYPOINT ["dotnet", "BlazorKukiji.Server.dll"]