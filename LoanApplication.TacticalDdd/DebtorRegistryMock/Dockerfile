FROM mcr.microsoft.com/dotnet/core/sdk:3.0
WORKDIR /app
COPY DebtorRegistryMock.csproj ./
RUN dotnet restore
COPY . .
CMD dotnet run