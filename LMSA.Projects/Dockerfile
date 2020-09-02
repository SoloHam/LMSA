#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["LMSA.Projects/LMSA.Projects.csproj", "/LMSA.Projects/"]
COPY ["../LMSA.Shared/LMSA.Shared.csproj", "../LMSA.Shared/"]
COPY ["../LMSA.Shared.Contracts/LMSA.Shared.Contracts.csproj", "../LMSA.Shared.Contracts/"]
COPY ["../LMSA.Shared.Consumers/LMSA.Shared.Consumers.csproj", "../LMSA.Shared.Consumers/"]
RUN dotnet restore "./LMSA.Projects/LMSA.Projects.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "/LMSA.Projects/LMSA.Projects.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/LMSA.Projects/LMSA.Projects.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /src/app/publish .
ENTRYPOINT ["dotnet", "LMSA.Projects.dll"]