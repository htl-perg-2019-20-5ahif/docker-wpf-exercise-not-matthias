#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CarBooking.API/CarBooking.API.csproj", "CarBooking.API/"]
RUN dotnet restore "CarBooking.API/CarBooking.API.csproj"
COPY . .
WORKDIR "/src/CarBooking.API"
RUN dotnet build "CarBooking.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarBooking.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarBooking.API.dll"]
