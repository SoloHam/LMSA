#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["LMSA.Gateway/LMSA.Gateway.csproj", "LMSA.Gateway/"]
COPY ["LMSA.Shared/LMSA.Shared.csproj", "LMSA.Shared/"]
RUN dotnet restore "LMSA.Gateway/LMSA.Gateway.csproj"
COPY . .
WORKDIR "/src/LMSA.Gateway"
RUN dotnet build "LMSA.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LMSA.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LMSA.Gateway.dll"]