FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DroneDelivery.Api/DroneDelivery.Api.csproj", "DroneDelivery.Api/"]
COPY ["DroneDelivery.Application/DroneDelivery.Application.csproj", "DroneDelivery.Application/"]
COPY ["DroneDelivery.Bus/DroneDelivery.Bus.csproj", "DroneDelivery.Bus/"]
COPY ["DroneDelivery.Data/DroneDelivery.Data.csproj", "DroneDelivery.Data/"]
COPY ["DroneDelivery.Domain/DroneDelivery.Domain.csproj", "DroneDelivery.Domain/"]
COPY ["DroneDelivery.Infra/DroneDelivery.Infra.csproj", "DroneDelivery.Infra/"]
COPY ["DroneDelivery.IOC/DroneDelivery.IOC.csproj", "DroneDelivery.IOC/"]
COPY ["DroneDelivery.Utility/DroneDelivery.Utility.csproj", "DroneDelivery.Utility/"]
COPY ["DroneDelivery.Domain.Core/DroneDelivery.Domain.Core.csproj", "DroneDelivery.Domain.Core/"]

RUN dotnet restore "DroneDelivery.Api/DroneDelivery.Api.csproj"
COPY . .
WORKDIR "/src/DroneDelivery.Api"
RUN dotnet build "DroneDelivery.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DroneDelivery.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "DroneDelivery.Api.dll" ]