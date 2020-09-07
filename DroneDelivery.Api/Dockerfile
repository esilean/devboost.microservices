FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DroneDelivery.Api/DroneDelivery.Api.csproj", "DroneDelivery.Api/"]
COPY ["DroneDelivery.Application/DroneDelivery.Application.csproj", "DroneDelivery.Application/"]
COPY ["DroneDelivery.Domain/DroneDelivery.Domain.csproj", "DroneDelivery.Domain/"]
COPY ["DroneDelivery.Data/DroneDelivery.Data.csproj", "DroneDelivery.Data/"]
COPY ["DroneDelivery.Infra/DroneDelivery.Infra.csproj", "DroneDelivery.Infra/"]
COPY ["DroneDelivery.IOC/DroneDelivery.IOC.csproj", "DroneDelivery.IOC/"]

COPY ["DroneDelivery.Shared.Bus/DroneDelivery.Shared.Bus.csproj", "DroneDelivery.Shared.Bus/"]
COPY ["DroneDelivery.Shared.Domain.Core/DroneDelivery.Shared.Domain.Core.csproj", "DroneDelivery.Shared.Domain.Core/"]
COPY ["DroneDelivery.Shared.Infra/DroneDelivery.Shared.Infra.csproj", "DroneDelivery.Shared.Infra/"]
COPY ["DroneDelivery.Shared.Utility/DroneDelivery.Shared.Utility.csproj", "DroneDelivery.Shared.Utility/"]


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