FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files for restore (handle referenced projects)
COPY ["src/4. Api/Atria.Api.csproj", "src/4. Api/"]
COPY ["src/2. Application/Atria.Application.csproj", "src/2. Application/"]
COPY ["src/3. Infrastructure/Atria.Infrastructure.csproj", "src/3. Infrastructure/"]

# Restore
RUN dotnet restore "src/4. Api/Atria.Api.csproj"

# Copy everything else
COPY . .

WORKDIR "src/4. Api"

# Build and publish
RUN dotnet publish "Atria.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Atria.Api.dll"]
