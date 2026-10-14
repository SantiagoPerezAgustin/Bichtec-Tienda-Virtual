FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solución y proyectos directamente
COPY ["BicTechBack.sln", "./"]
COPY ["src/Presentation/Presentation.csproj", "src/Presentation/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "src/Presentation/Presentation.csproj"

# Copiar todo el código restante
COPY . .

# Publicar
WORKDIR "/src/src/Presentation"
RUN dotnet publish "Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final de ejecucion
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Presentation.dll"]
