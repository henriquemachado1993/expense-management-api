#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ExpenseApi/ExpenseApi.csproj", "ExpenseApi/"]
COPY ["ExpenseApi.Infra/ExpenseApi.Infra.csproj", "ExpenseApi.Infra/"]
COPY ["ExpenseApi.Domain/ExpenseApi.Domain.csproj", "ExpenseApi.Domain/"]
COPY ["ExpenseApi.Service/ExpenseApi.Service.csproj", "ExpenseApi.Service/"]
RUN dotnet restore "ExpenseApi/ExpenseApi.csproj"
COPY . .
WORKDIR "/src/ExpenseApi"
RUN dotnet build "ExpenseApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpenseApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpenseApi.dll"]
