#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY devkey.pfx devkey.pfx

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NarwianskiZakatek/NarwianskiZakatek.csproj", "NarwianskiZakatek/"]
RUN dotnet restore "NarwianskiZakatek/NarwianskiZakatek.csproj"
COPY . .
WORKDIR "/src/NarwianskiZakatek"
RUN dotnet build "NarwianskiZakatek.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NarwianskiZakatek.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NarwianskiZakatek.dll"]
