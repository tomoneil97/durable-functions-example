FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY src .
RUN dotnet restore src.csproj
RUN dotnet build src.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish src.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated7.0 AS final
WORKDIR /home/site/wwwroot
EXPOSE 80
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
AzureWebJobsStorage=$AzureWebJobsStorage \
BlobContainerName=$BlobContainerName \
BlobOutputContainerName=$BlobOutputContainerName \
AzureFunctionsJobHost__Logging__Console__IsEnabled=true 