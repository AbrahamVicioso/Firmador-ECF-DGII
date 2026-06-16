FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Firmador-API/Firmador-API.csproj", "Firmador-API/"]
RUN dotnet restore "Firmador-API/Firmador-API.csproj"
COPY . .
WORKDIR "/src/Firmador-API"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5299
ENV ASPNETCORE_URLS=http://+:5299
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "Firmador-API.dll"]
