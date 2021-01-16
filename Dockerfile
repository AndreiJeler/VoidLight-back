FROM mcr.microsoft.com/dotnet/sdk:5.0.102 AS build-env
WORKDIR /app
# RUN apt-get install -y icu-devtools
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:5.0.102-alpine3.12
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 44324
EXPOSE 5000
EXPOSE 5001
ENTRYPOINT ["dotnet", "VoidLight.Web.dll"]
