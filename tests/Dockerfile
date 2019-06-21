FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build-env

# based on https://github.com/dotnet/dotnet-docker/issues/632
RUN \
    apk update \
    && apk add \
        bash \
    ;

# copy build scripts
COPY ./fake.sh /service-identification/
COPY ./build.fsx /service-identification/
COPY ./paket.dependencies /service-identification/
COPY ./paket.lock /service-identification/

# copy app
COPY ./ServiceIdentification.fsproj /service-identification/
COPY ./src /service-identification/src

# copy tests
COPY ./tests/MatchingServiceIdentification.fs /service-identification/tests/MatchingServiceIdentification.fs
COPY ./tests/Tests.fs /service-identification/tests/Tests.fs
COPY ./tests/tests.fsproj /service-identification/tests/tests.fsproj

WORKDIR /service-identification/tests

RUN \
    dotnet restore \
    && dotnet publish -c Release -o /tests \
    ;

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.2-alpine

# Install runtime dependencies
RUN \
    apk update \
    && apk add \
        binutils \
        # Install bash
        bash \
        bash-completion \
        bash-doc \
        # Install extensions dependencies
        curl \
    ;

## Install LMC linca root certificate
RUN \
    curl -sL -o lmc-certs.deb http://lmcinfra-apt.service.cprod.consul/lmc-sys/pool/main/l/lmc-certs/lmc-certs_2019.06.13-1.0.0.2.g46b9dce_all.deb \
    && ar -x lmc-certs.deb \
    && tar -xvf data.tar.gz \
    && cp usr/local/share/ca-certificates/LMC-LINCA/lmc_linca_root.crt /usr/local/share/ca-certificates/lmc_linca_root.crt \
    && cp usr/local/share/ca-certificates/LMC-AD/lmc_ad_root.crt /usr/local/share/ca-certificates/lmc_ad_root.crt \
    && update-ca-certificates \
    ;

WORKDIR /tests
COPY --from=build-env /tests .

CMD ["dotnet", "tests.dll"]
