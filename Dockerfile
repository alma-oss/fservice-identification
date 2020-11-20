FROM dcreg.service.consul/dev/development-dotnet-core-sdk-common:5.0

# build scripts
COPY ./build.sh /fservice-identification/
COPY ./build.fsx /fservice-identification/
COPY ./paket.dependencies /fservice-identification/
COPY ./paket.references /fservice-identification/
COPY ./paket.lock /fservice-identification/

# sources
COPY ./ServiceIdentification.fsproj /fservice-identification/
COPY ./src /fservice-identification/src

# copy tests
COPY ./tests /fservice-identification/tests

# others
COPY ./.config /fservice-identification/.config
COPY ./.git /fservice-identification/.git
COPY ./CHANGELOG.md /fservice-identification/

WORKDIR /fservice-identification

RUN \
    ./build.sh -t Build no-clean

CMD ["./build.sh", "-t", "Tests", "no-clean"]
