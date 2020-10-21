ARG NODE_IMAGE=node:latest

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.2-buster-slim AS base
WORKDIR /app

# 1. NUGET RESTORE

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.102 AS build
WORKDIR /src
COPY ./GeoInfoApp.sln ./
COPY ./GeoInfoApp/. ./GeoInfoApp/

RUN dotnet restore

# 2.1 BUILD Net Core app


RUN dotnet build ./GeoInfoApp/GeoInfoApp.csproj -o /app/build

####
# Deploy database


###

#2.2 BUILD angular app

FROM ${NODE_IMAGE} as node-build
WORKDIR /src
COPY ./GeoInfoApp/ClientApp .
RUN npm install
RUN npm run build -- --prod

# 3 TODO: tests

# 5 Publish
FROM build AS publish
RUN dotnet publish ./GeoInfoApp/GeoInfoApp.csproj -o /app/publish

# entry point
FROM base AS final
WORKDIR /app
COPY --from=publish /app/build .
COPY --from=node-build /src/dist ./ClientApp/dist

CMD ["dotnet", "GeoInfoApp.dll"]