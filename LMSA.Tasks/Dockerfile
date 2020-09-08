#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["LMSA.Tasks/LMSA.Tasks.csproj", "/LMSA.Tasks/"]
COPY ["LMSA.Shared/LMSA.Shared.csproj", "/LMSA.Shared/"]
RUN dotnet restore "/LMSA.Tasks/LMSA.Tasks.csproj"
COPY . .
WORKDIR "/src/LMSA.Tasks"
RUN dotnet build "LMSA.Tasks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LMSA.Tasks.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LMSA.Tasks.dll"]