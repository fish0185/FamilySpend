# --- Stage 1: Build and Publish ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
WORKDIR "/src/FamilySpend"
RUN dotnet restore "FamilySpend.csproj"
RUN dotnet publish -c Release -o /app/publish --no-restore


# --- Stage 2: Final Runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Create a secure non-root system user for production safety
RUN useradd --create-home --shell /bin/bash appuser
USER appuser

# Copy published binaries from the build stage
COPY --from=build /app/publish .

# ASP.NET Core 8.0+ / 10.0 binds to port 8080 by default for non-root users
EXPOSE 8080

ENTRYPOINT ["dotnet", "FamilySpend.dll"]