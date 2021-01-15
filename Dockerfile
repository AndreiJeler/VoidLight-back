FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . ./
COPY ../../../../../../../../../../../../home/polk15/voidlight/appsettings.json ./VoidLight.Web/
RUN dotnet restore

RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 44324
ENTRYPOINT ["dotnet", "VoidLight.Web.dll"]
