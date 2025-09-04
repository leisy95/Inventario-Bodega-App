# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copiamos solo los archivos de proyecto primero (mejor para la caché de Docker)
COPY *.sln ./
COPY *.csproj ./

# Restauramos dependencias
RUN dotnet restore ./InventarioBackend.csproj

# Copiamos el resto del código
COPY . ./

# Compilamos
RUN dotnet build ./InventarioBackend.csproj -c Release -o out

# Etapa publish
FROM build AS publish
RUN dotnet publish ./InventarioBackend.csproj -c Release -o out

# Etapa runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=publish /app/out .
ENTRYPOINT ["dotnet", "InventarioBackend.dll"]