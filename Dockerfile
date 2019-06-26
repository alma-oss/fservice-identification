FROM dcreg.service.consul/prod/development-dotnet-core-sdk-common:latest

# build scripts
COPY ./fake.sh /fservice-identification/
COPY ./build.fsx /fservice-identification/
COPY ./paket.dependencies /fservice-identification/
COPY ./paket.references /fservice-identification/
COPY ./paket.lock /fservice-identification/

# sources
COPY ./ServiceIdentification.fsproj /fservice-identification/
COPY ./src /fservice-identification/src

# copy tests
COPY ./tests /fservice-identification/tests

WORKDIR /fservice-identification

RUN \
    ./fake.sh build target Build no-clean

CMD ["./fake.sh", "build", "target", "Tests", "no-clean"]
