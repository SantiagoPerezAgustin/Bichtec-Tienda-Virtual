FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solución y proyectos incluyendo la carpeta contenedora
COPY ["BicTechBack/BicTechBack.sln", "./"]
COPY ["BicTechBack/src/Presentation/Presentation.csproj", "BicTechBack/src/Presentation/"]
COPY ["BicTechBack/src/Application/Application.csproj", "BicTechBack/src/Application/"]
COPY ["BicTechBack/src/Domain/Domain.csproj", "BicTechBack/src/Domain/"]
COPY ["BicTechBack/src/Infrastructure/Infrastructure.csproj", "BicTechBack/src/Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "BicTechBack/src/Presentation/Presentation.csproj"

# Copiar todo el código
COPY BicTechBack/ ./BicTechBack/

# Publicar
WORKDIR "/src/BicTechBack/src/Presentation"
RUN dotnet publish "Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final de ejecucion
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Presentation.dll"]
