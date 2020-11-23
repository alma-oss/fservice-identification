# Changelog

<!-- There is always Unreleased section on the top. Subsections (Add, Changed, Fix, Removed) should be Add as needed. -->
## Unreleased

## 5.0.1 - 2020-11-23
- Fix paket dependencies

## 5.0.0 - 2020-11-20
- Use .netcore 5.0

## 4.0.0 - 2020-11-19
- [**BC**] Change namespace to `Lmc.ServiceIdentification`
- Add create functions `createFromValues` and `createFromStrings` to modules:
    - `Service`
    - `Processor`
    - `Instance`
    - `Spot`
    - `Box`
    - `BoxPattern`
- Use .netcore 3.1

## 3.4.0 - 2020-03-02
- Add pattern types and modules
    - `BoxPattern`
    - `PurposePattern`
    - `VersionPattern`
    - `ZonePattern`
    - `BucketPattern`

## 3.3.0 - 2020-01-14
- Add function to access inner values
    - `ServiceIdentification.service`
- Update dependencies
- Add `AssemblyInfo`

## 3.2.0 - 2019-11-07
- Add functions to access inner values
    - `Service.domain`
    - `Service.context`
    - `Processor.purpose`
    - `Instance.version`
    - `Spot.zone`
    - `Spot.bucket`
- Change git host

## 3.1.0 - 2019-08-21
- Add more create functions
    - `Processor.ofService`
    - `Instance.ofService`
    - `Instance.ofProcessor`
    - `Box.ofService`
    - `Box.ofProcessor`
    - `Box.ofInstance`

## 3.0.0 - 2019-08-12
- Add `ServiceIdentification.isMatching` function

## 2.6.0 - 2019-08-12
- Change `ServiceIdentification` module
    - Add `concat` function
    - [**BC**] Remove `isMatchingUpstreamDependency` function

## 2.5.0 - 2019-06-24
- Add lint

## 2.4.0 - 2019-06-21
- Add `ServiceIdentification` type and module
- Update build dependencies, add Lint and Tests

## 2.3.0 - 2019-06-12
- Add more transformation functions
    - `Instance.service`
    - `Instance.processor`
    - `Processor.service`

## 2.2.0 - 2019-06-07
- Add `Box.spot` function

## 2.1.0 - 2019-05-27
- Add function to parse Spot from string

## 2.0.0 - 2019-03-13
- Add function to create Box from values
- [**BC**] Remove `Box.concat` function
- Add `Box.instance` function
- Add `Instance.concat` function

## 1.2.0 - 2019-03-06
- Add function to parse Instance from string

## 1.1.0 - 2019-02-12
- Add `Box` module with functions
    - `createFromStrings`
    - `concat`

## 1.0.0 - 2019-01-31
- Initial implementation
