FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Container to compile angular part
FROM node:16-alpine AS client 
ARG strict_ssl=false
ARG skip_client_build=false

WORKDIR /app 
COPY ClientApp .

RUN [[ ${skip_client_build} = true ]] && echo "Skipping npm install" || npm install --legacy-peer-deps
RUN [[ ${skip_client_build} = true ]] && mkdir dist || npm run build -- --prod

# Container to compile dotnet part
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY . .

# Compile CSharp part
RUN dotnet restore DotnetCoreSampleA.csproj
RUN dotnet build DotnetCoreSampleA.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotnetCoreSampleA.csproj" -c Release -o /app/publish /p:BuildAngular=false

FROM base AS final
RUN apt-get update && apt-get install -y \
    git \
    nodejs \
    npm \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=client /app/dist /app/dist
RUN mkdir StaticFiles
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS="http://0.0.0.0:80"
ENV ConnectionStrings__DefaultConnection="Server=(local);Database=DotnetCoreSampleA_DB;Trusted_Connection=True;MultipleActiveResultSets=true;User ID=sa;Password=1325897"
ENTRYPOINT ["dotnet", "DotnetCoreSampleA.dll"]