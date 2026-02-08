# ---------- BUILD STAGE ----------
FROM mcr.microsoft.com/dotnet/nightly/sdk:10.0 AS build
WORKDIR /src

# Copiamos solo el proyecto primero (mejora cache)
COPY CarRental.Api/CarRental.Api.csproj CarRental.Api/
RUN dotnet restore CarRental.Api/CarRental.Api.csproj

# Copiamos el resto del código
COPY . .

# Publicamos
RUN dotnet publish CarRental.Api/CarRental.Api.csproj -c Release -o /app/publish

# ---------- RUNTIME STAGE ----------
FROM mcr.microsoft.com/dotnet/nightly/aspnet:10.0
WORKDIR /app

# Librería necesaria para Npgsql/PostgreSQL (evita libgssapi_krb5.so.2 error)
RUN apt-get update && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

# Copiamos el publish
COPY --from=build /app/publish .

# Render usa la variable PORT
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CarRental.Api.dll"]
