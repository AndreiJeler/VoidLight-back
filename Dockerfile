FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app
# RUN apt-get install -y icu-devtools
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 44324
ENTRYPOINT ["dotnet", "VoidLight.Web.dll"]
