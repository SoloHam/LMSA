FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster As build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o dist

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim As final
WORKDIR /app
COPY --from=build /src/dist .
ENTRYPOINT [ "dotnet", "LMSA.Projects.dll" ]
