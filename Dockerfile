# Use the official .NET Core SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# Copy the necessary files and restore dependencies
COPY . .

# Run dotnet restore for the solution
RUN dotnet restore "EmpowerIdMicroservice.WebApi/EmpowerIdMicroservice.WebApi.csproj"

# Publish the application
RUN dotnet publish "EmpowerIdMicroservice.WebApi/EmpowerIdMicroservice.WebApi.csproj" -c release -o ./publish --no-restore

# Use the smaller runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Copy the published output from the build image
COPY --from=build /app .

# Expose the port on which the application will run
EXPOSE 7041

# Set the entry point for the application
ENTRYPOINT ["dotnet", "EmpowerIdMicroservice.WebApi.dll"]