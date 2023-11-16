FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7041

ENV ASPNETCORE_URLS=http://+:7041

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
#RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
#USER appuser

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
#COPY ["EmpowerIdMicroservice.WebApi/EmpowerIdMicroservice.WebApi.csproj", "EmpowerIdMicroservice.WebApi/"]
COPY . .
RUN dotnet restore "EmpowerIdMicroservice.WebApi/EmpowerIdMicroservice.WebApi.csproj"
COPY . .
WORKDIR "/src/EmpowerIdMicroservice.WebApi"
RUN dotnet build "EmpowerIdMicroservice.WebApi.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "EmpowerIdMicroservice.WebApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmpowerIdMicroservice.WebApi.dll"]