# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY . .
RUN dotnet restore --use-current-runtime  

WORKDIR /TodoApp.WebApi
RUN dotnet publish -c release -o /app --use-current-runtime --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "TodoApp.WebApi.dll"]

