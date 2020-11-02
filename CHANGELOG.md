# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.2] - 2020-11-02
### Added
- Drag & Drop demo.
- Helper method `DiagramManager.GetRelativePoint`.

### Fixed
- Diagram container resizes don't update the offsets/position (top/left).
- Container changes don't trigger node visibility checks.
- Ports aren't refreshed when nodes are resized.

## [1.1.0] - 2020-10-19
### Added
- Track mouseup events on nodes [@joriskalz](https://github.com/joriskalz).
- Zooming in/out will now trigger the nodes visibility check.
- `AllowZooming` and `AllowPanning` options.
- `DefaultColor` and `DefaultSelectedColor` link options.
- Custom ports/links demos/documentation.
- Options documentation.

### Changed
- Group link related options into `DiagramLinkOptions`, available in `DiagramOptions.Links`.
- All link related calculations (e.g. `MiddleSourceX`) are now extension methods available in `LinkModelExtensions`.

### Removed
- `LinkWidget`'s behind file is not needed anymore.
