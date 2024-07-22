#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BeymenCaseAPI/BeymenCaseAPI.csproj", "BeymenCaseAPI/"]
COPY ["BaymenCase.ConfigurationReader/BeymenCase.ConfigurationReader.csproj", "BaymenCase.ConfigurationReader/"]
COPY ["BaymenCase.Redis/BeymenCase.Redis.csproj", "BaymenCase.Redis/"]
COPY ["BeymenCase.Common/BeymenCase.Common.csproj", "BeymenCase.Common/"]
COPY ["BeymenCaseAPI.Application/BeymenCaseAPI.Application.csproj", "BeymenCaseAPI.Application/"]
COPY ["BeymenCaseAPI.Infrastructer/BeymenCaseAPI.Infrastructer.csproj", "BeymenCaseAPI.Infrastructer/"]
RUN dotnet restore "./BeymenCaseAPI/BeymenCaseAPI.csproj"
COPY . .
WORKDIR "/src/BeymenCaseAPI"
RUN dotnet build "./BeymenCaseAPI.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BeymenCaseAPI.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BeymenCaseAPI.dll"]