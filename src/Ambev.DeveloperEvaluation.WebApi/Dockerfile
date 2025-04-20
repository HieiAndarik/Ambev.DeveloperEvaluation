FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
USER root
WORKDIR /src

COPY . .

RUN chmod -R 755 /src

RUN apt-get update && apt-get install -y openssl

RUN dotnet restore "/src/src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"

WORKDIR "/src/src/Ambev.DeveloperEvaluation.WebApi"
RUN dotnet build "Ambev.DeveloperEvaluation.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ambev.DeveloperEvaluation.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /https


ENTRYPOINT ["dotnet", "Ambev.DeveloperEvaluation.WebApi.dll"]