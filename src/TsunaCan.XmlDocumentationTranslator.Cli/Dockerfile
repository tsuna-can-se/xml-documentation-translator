# デバッグ コンテナーをカスタマイズする方法と、Visual Studio がこの Dockerfile を使用してより高速なデバッグのためにイメージをビルドする方法については、https://aka.ms/customizecontainer をご覧ください。

# このステージは、VS から高速モードで実行するときに使用されます (デバッグ構成の既定値)
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app


# このステージは、サービス プロジェクトのビルドに使用されます
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Directory.Build.props", "src/"]
COPY ["Directory.Build.props", "."]
COPY ["src/TsunaCan.XmlDocumentationTranslator.Cli/TsunaCan.XmlDocumentationTranslator.Cli.csproj", "src/TsunaCan.XmlDocumentationTranslator.Cli/"]
RUN dotnet restore "./src/TsunaCan.XmlDocumentationTranslator.Cli/TsunaCan.XmlDocumentationTranslator.Cli.csproj"
COPY . .
WORKDIR "/src/src/TsunaCan.XmlDocumentationTranslator.Cli"
RUN dotnet build "./TsunaCan.XmlDocumentationTranslator.Cli.csproj" -c $BUILD_CONFIGURATION -o /app/build

# このステージは、最終ステージにコピーするサービス プロジェクトを公開するために使用されます
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TsunaCan.XmlDocumentationTranslator.Cli.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# このステージは、運用環境または VS から通常モードで実行している場合に使用されます (デバッグ構成を使用しない場合の既定)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TsunaCan.XmlDocumentationTranslator.Cli.dll"]