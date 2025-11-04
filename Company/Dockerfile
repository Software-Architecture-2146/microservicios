# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos SOLO los csproj primero para aprovechar la cache de restore
COPY Company/Company.csproj Company/
COPY building-blocks/Contracts/Contracts.csproj building-blocks/Contracts/

# Restaura con las rutas correctas
RUN dotnet restore Company/Company.csproj

# Ahora sí copia todo el repo (necesario para compilar)
COPY . .

# Publica la API de Company
RUN dotnet publish Company/Company.csproj -c Release -o /app

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Company.dll"]
