
FROM mcr.microsoft.com/dotnet/aspnet:9.0
COPY Release App/
WORKDIR /App

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "photoContainer.dll"]


# Build stage
#FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#WORKDIR /src

#COPY *.sln ./
#COPY video2/*.csproj ./video2/
#RUN dotnet restore ./video2/video2.csproj

#COPY video2/. ./video2/
#WORKDIR /src/video2
#RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
#FROM mcr.microsoft.com/dotnet/aspnet:9.0
#WORKDIR /app

#ENV ASPNETCORE_URLS=http://+:8080
#EXPOSE 8080

#COPY --from=build /app/publish .

#ENTRYPOINT ["dotnet", "video2.dll"]