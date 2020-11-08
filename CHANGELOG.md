# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Diagrams [1.2.0] - 2020-11-08
### Added
- `DefaultLinkModel` in `DiagramLinkOptions` ([@joezearing](https://github.com/joezearing)).
- Ability to change source/target ports of a link (`SetSourcePort` and `SetTargetPort`).  
The methods also trigger the new events `SourcePortChanged` and `TargetPortChanged`.

### Changed
- Curved link paths now take into account the ports alignements and a margin ([@joezearing](https://github.com/joezearing)).
- The `AddLink<T>(T link, PortModel source, PortModel? target = null)` overload in `DiagramManager` is now public.  
This is useful when developers want to create the link instance themselves, `DiagramMananger` will setup ports and trigger appropriate events.

### Fixed
- `GetMiddleTargetX` and `GetMiddleTargetY` using `SourcePort` instead of `TargetPort` ([@joezearing](https://github.com/joezearing)).

## Algorithms [0.1.0] - 2020-11-08
A new project that aims to group all the algorithms that can be applied to `DiagramManager`.  
It's a seperate package so that you only include it when you need it.

### Added
- Reconnect links to the closest ports (Idea & Initial work by [@kolbjornb](https://github.com/kolbjornb)).

## [1.1.3] - 2020-11-03
### Fixed
- Diagram Container not ready when ports/nodes need it.

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
