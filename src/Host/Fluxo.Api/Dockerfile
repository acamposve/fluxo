# Single image for every deployment target (Azure or on-premise, constitution.md Artículo X).
# No target-specific branching in here — only environment variables at `docker run`/Compose time.

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Fluxo.sln .
COPY src/BuildingBlocks/ src/BuildingBlocks/
COPY src/Modules/ src/Modules/
COPY src/Host/Fluxo.Api/ src/Host/Fluxo.Api/
RUN dotnet publish src/Host/Fluxo.Api/Fluxo.Api.csproj -c Release -o /app --no-self-contained

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Fluxo.Api.dll"]
