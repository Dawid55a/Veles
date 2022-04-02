FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VelesServer/VelesServer.csproj", "VelesServer/"]
RUN dotnet restore "VelesServer/VelesServer.csproj"
COPY . .
WORKDIR "/src/VelesServer"
RUN dotnet build "VelesServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VelesServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VelesServer.dll"]
