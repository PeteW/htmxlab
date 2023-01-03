# this docker file is customized for running .net apps on heroku using the container feature.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# copy csproj first and restore it
COPY *.csproj ./
RUN dotnet restore

# copy remaining files and perform build
COPY . .
RUN dotnet publish -c Release -o out

# begin building runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

# ENTRYPOINT [ "dotnet", "E2.dll" ]
# NOTE: for heroku we need to map the provided port to the app port hence the custom CMD entry below
CMD ASPNETCORE_URLS=http://*:$PORT dotnet E2.dll
