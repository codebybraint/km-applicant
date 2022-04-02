FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["km-applicant-backend.csproj", "./"]
RUN dotnet restore "km-applicant-backend.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "km-applicant-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "km-applicant-backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ADD Databases/ /app/Databases/
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "km-applicant-backend.dll"]