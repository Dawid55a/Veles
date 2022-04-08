#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 53353/tcp
EXPOSE 53353/udp

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
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