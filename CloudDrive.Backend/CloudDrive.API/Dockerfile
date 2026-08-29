FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5214

ENV ASPNETCORE_URLS=http://+:5214

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["CloudDrive.Backend/CloudDrive.API/CloudDrive.API.csproj", "CloudDrive.Backend/CloudDrive.API/"]
RUN dotnet restore "CloudDrive.Backend/CloudDrive.API/CloudDrive.API.csproj"
COPY . .
WORKDIR "/src/CloudDrive.Backend/CloudDrive.API"
RUN dotnet build "CloudDrive.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "CloudDrive.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudDrive.API.dll"]
