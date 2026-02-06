# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiamos el csproj y restauramos dependencias
COPY CarRental.Api.csproj ./
RUN dotnet restore

# Copiamos todo y publicamos
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080

ENTRYPOINT ["dotnet", "CarRental.Api.dll"]
