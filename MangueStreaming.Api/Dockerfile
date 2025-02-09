# Base para execução da aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar os arquivos de projeto usando caminhos relativos a partir da raiz do contexto
COPY ["MangueStreaming.Api/MangueStreaming.Api.csproj", "MangueStreaming.Api/"]
COPY ["MangueStreaming.Application/MangueStreaming.Application.csproj", "MangueStreaming.Application/"]
COPY ["MangueStreaming.Domain/MangueStreaming.Domain.csproj", "MangueStreaming.Domain/"]
COPY ["MangueStreaming.Infra/MangueStreaming.Infra.csproj", "MangueStreaming.Infra/"]

# Restaurar as dependências
RUN dotnet restore "MangueStreaming.Api/MangueStreaming.Api.csproj"

# Copiar o restante do código-fonte
COPY . .

# Definir o diretório de trabalho para o projeto da API e compilar
WORKDIR "/src/MangueStreaming.Api"
RUN dotnet build "MangueStreaming.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MangueStreaming.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa Final: Imagem de Execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MangueStreaming.Api.dll"]
