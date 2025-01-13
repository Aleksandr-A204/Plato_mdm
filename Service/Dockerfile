# Этот этап используется для сборки проекта клиентской службы
FROM node:lts AS client
RUN npm config set registry https://nexus.infolan.org/repository/npm-proxy/ # прокси для ускорения npm install из локальной сети
WORKDIR /app

COPY ClientApp/package.json package.json
COPY ClientApp/package-lock.json package-lock.json
RUN npm install

COPY ClientApp .

ENV NODE_ENV=production
RUN npm run build

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Этот этап используется для сборки проекта сервисной службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN dotnet nuget remove source nuget.org
RUN dotnet nuget add source https://nexus.infolan.org/repository/nuget-group/index.json -n LocalNexus

WORKDIR /src
COPY ["Plato.MDM.csproj", "."]
RUN dotnet restore "./Plato.MDM.csproj"
COPY . .
COPY --from=client /app/dist wwwroot
RUN dotnet build "./Plato.MDM.csproj" -c Release -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
RUN dotnet publish "./Plato.MDM.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Plato.MDM.dll"]