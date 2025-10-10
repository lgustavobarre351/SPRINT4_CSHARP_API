# Use the official .NET 9.0 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET 9.0 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Investimentos.csproj", "."]
RUN dotnet restore "Investimentos.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Investimentos.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Investimentos.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "Investimentos.dll"]