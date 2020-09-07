FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DroneDelivery.Pagamento.Api/DroneDelivery.Pagamento.Api.csproj", "DroneDelivery.Pagamento.Api/"]
COPY ["DroneDelivery.Pagamento.Application/DroneDelivery.Pagamento.Application.csproj", "DroneDelivery.Pagamento.Application/"]
COPY ["DroneDelivery.Pagamento.Domain/DroneDelivery.Pagamento.Domain.csproj", "DroneDelivery.Pagamento.Domain/"]
COPY ["DroneDelivery.Pagamento.Data/DroneDelivery.Pagamento.Data.csproj", "DroneDelivery.Pagamento.Data/"]
COPY ["DroneDelivery.Pagamento.Gateway/DroneDelivery.Pagamento.Gateway.csproj", "DroneDelivery.Pagamento.Gateway/"]
COPY ["DroneDelivery.Pagamento.IOC/DroneDelivery.Pagamento.IOC.csproj", "DroneDelivery.Pagamento.IOC/"]

COPY ["DroneDelivery.Shared.Bus/DroneDelivery.Shared.Bus.csproj", "DroneDelivery.Shared.Bus/"]
COPY ["DroneDelivery.Shared.Domain.Core/DroneDelivery.Shared.Domain.Core.csproj", "DroneDelivery.Shared.Domain.Core/"]
COPY ["DroneDelivery.Shared.Infra/DroneDelivery.Shared.Infra.csproj", "DroneDelivery.Shared.Infra/"]
COPY ["DroneDelivery.Shared.Utility/DroneDelivery.Shared.Utility.csproj", "DroneDelivery.Shared.Utility/"]


RUN dotnet restore "DroneDelivery.Pagamento.Api/DroneDelivery.Pagamento.Api.csproj"
COPY . .
WORKDIR "/src/DroneDelivery.Pagamento.Api"
RUN dotnet build "DroneDelivery.Pagamento.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DroneDelivery.Pagamento.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "DroneDelivery.Pagamento.Api.dll" ]