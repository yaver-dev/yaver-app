# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.48.0] - 2024-05-20

### Changed

- ReplicableEntity removed from the project.
- ReplicableEntityInterceptor removed from the project.

## [0.47.0] - 2024-05-19

### Changed

- ReplicableEntity added to the project which is a base class for entities that can be replicated from cloud to edge or from edge to cloud directions both.
- version bump for various nuget packages

## [0.46.0] - 2024-04-03

### Changed

- version bump for various nuget packages

## [0.45.1] - 2024-02-24

### Changed

- version bump for release

## [0.45.0] - 2024-02-24

### Changed

- TenantInfo record name property changed to identifier.
- Minor refactor on sample projects in order to increase readability.

## [0.42.1] - 2023-12-20

### Changed

- fix appsecrets.json file path on web application builder extension

## [0.42.0] - 2023-12-18

### Added

- Localization support on validation errors
- Tenant context support for multi-tenant applications

### Changed

- Request info enrichment for audit logs

## [0.41.0] - 2023-12-13

### Fixed

- map rpc handlers method was not working properly on bff apis, new implementation added

### Changed

- request info modified tenant added
- userInfoAuthenticationHandler implementation changed, tenant information fetched from claim instead of header

### Removed

- request info modified tenantIdentifier no longer exists
