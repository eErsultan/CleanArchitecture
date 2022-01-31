FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src

COPY ["src/WebAPI/WebAPI.csproj", "src/WebAPI/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/WebAPI/WebAPI.csproj"

COPY . .
WORKDIR "/src/src/WebAPI"
FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=$APP_ENVIRONMENT 
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "WebAPI.dll"]
