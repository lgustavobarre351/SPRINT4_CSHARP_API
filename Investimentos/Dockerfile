# Dockerfile para Render - contexto na pasta pai
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Debug: listar arquivos
RUN echo "=== Listando arquivos do contexto ==="
COPY . .
RUN ls -la
RUN echo "=== Procurando arquivo .csproj ==="
RUN find . -name "*.csproj" -type f

# Restore e build (arquivo está dentro da pasta Investimentos)
WORKDIR /src/Investimentos
RUN dotnet restore Investimentos.csproj
RUN dotnet build Investimentos.csproj -c Release -o /app/build

# Publish
RUN dotnet publish Investimentos.csproj -c Release -o /app/publish

# Final stage - runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Configure environment
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 10000
ENTRYPOINT ["dotnet", "Investimentos.dll"]