FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build-env

# based on https://github.com/dotnet/dotnet-docker/issues/632
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

ENV PATH="${PATH}:/root/.dotnet/tools"

# build scripts
COPY ./fake.sh /fservice-identification/
COPY ./build.fsx /fservice-identification/
COPY ./paket.dependencies /fservice-identification/
COPY ./paket.lock /fservice-identification/

# sources
COPY ./ServiceIdentification.fsproj /fservice-identification/
COPY ./src /fservice-identification/src

# copy tests
COPY ./tests /fservice-identification/tests

WORKDIR /fservice-identification

RUN \
    ./fake.sh build target Build

CMD ["./fake.sh", "build", "target", "Tests"]
