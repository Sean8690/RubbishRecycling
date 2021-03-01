# Running locally:
# docker build -t infotrack-cdd:1.0.0 .
# docker run -d -p 8080:80 --name infotrack-cdd -e ASPNETCORE_ENVIRONMENT=Stage -e SQLCONNSTR_LDMCORE='Data Source=ldmstagedb.infotrack.com.au;Initial Catalog=LdmCore;User Id=LDMSSecure;Password=V7497HMrkPDOdr2EBE5J;' -e OAUTH__ENDPOINT=https://stageauth.infotrack.com.au infotrack-cdd:1.0.0

# Stage 1
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

WORKDIR /app

COPY ./InfoTrack.Cdd.sln .
COPY ./NuGet.config .
COPY src/InfoTrack.Cdd.Api/InfoTrack.Cdd.Api.csproj ./src/InfoTrack.Cdd.Api/
COPY src/InfoTrack.Cdd.Api.Client/InfoTrack.Cdd.Api.Client.csproj ./src/InfoTrack.Cdd.Api.Client/
COPY src/InfoTrack.Cdd.Application/InfoTrack.Cdd.Application.csproj ./src/InfoTrack.Cdd.Application/
COPY src/InfoTrack.Cdd.Infrastructure/InfoTrack.Cdd.Infrastructure.csproj ./src/InfoTrack.Cdd.Infrastructure/
COPY test/InfoTrack.Cdd.Api.IntegrationTests/InfoTrack.Cdd.Api.IntegrationTests.csproj ./test/InfoTrack.Cdd.Api.IntegrationTests/
COPY test/InfoTrack.Cdd.Application.UnitTests/InfoTrack.Cdd.Application.UnitTests.csproj ./test/InfoTrack.Cdd.Application.UnitTests/

# Restore as distinct layers.
RUN dotnet restore 

# Copy everything else
COPY . .

# Run integration tests (these are good locally but don't seem to run remotely)
# RUN dotnet test test/InfoTrack.Cdd.Api.IntegrationTests/InfoTrack.Cdd.Api.IntegrationTests.csproj --logger console --verbosity normal

# Run code coverage
ARG code_coverage_percentage=0
# RUN dotnet test test/InfoTrack.Cdd.Application.UnitTests/InfoTrack.Cdd.Application.UnitTests.csproj -l trx -r ../CodeCoverage/TestResults /p:CollectCoverage=true /p:CoverletOutput=..\InfoTrack.Cdd.Application.UnitTests\TestResults\Coverage\InfoTrack.Cdd.Application.UnitTests.json /p:CoverletOutput=../InfoTrack.Cdd.Application.UnitTests/TestResults/Coverage/coverage.opencover.xml /p:CoverletOutputFormat=opencover /p:Threshold=${code_coverage_percentage}
RUN dotnet test test/InfoTrack.Cdd.Application.UnitTests/InfoTrack.Cdd.Application.UnitTests.csproj -l trx /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Threshold=${code_coverage_percentage}

# Coverage Report Generation
# RUN /dotnetglobaltools/reportgenerator "-reports:./*/TestResults/Coverage/coverage.opencover.xml" -targetdir:./TestResults/Coverage/Reports -reporttypes:Html

# Publish API
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine

# Required for Time Zone database lookups
RUN apk add tzdata
# Set up timezone and locale for proper date format etc.
ENV TZ=Australia/Sydney
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
ENV LANG en_AU.UTF-8
ENV LANGUAGE ${LANG}
ENV LC_ALL ${LANG}

# Fix SQL client globalization
RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app
COPY --from=build-env /app/out .
# COPY --from=build-env /test/InfoTrack.Cdd.Application.UnitTests/TestResults ./TestResults/Coverage
# COPY --from=build-env /src/api/TestResults/Coverage/Reports ./TestResults/Coverage/Reports

ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "InfoTrack.Cdd.Api.dll"]