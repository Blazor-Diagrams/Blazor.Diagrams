# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Diagrams [1.5.0] - 2021-01-05

## Added

- The ability to have ports on groups.
- **EXPERIMENTAL/INCOMPLETE** Nested groups. Since `GroupModel` now inherits `NodeMode`, it became possible to have nested groups, but there are still problems with the order of links between groups.
- A `Class` parameter to `GroupContainer`.

## Changed

- Only rerender groups when necessary.
- Receiving the same size from `ResizeObserver` doesn't trigger a rerender anymore.
- Avoid rerendering ports twice to update positions.
- Avoid rerendering ports when their parent node is moving.
- Padding is now handled in `GroupModel` instead of `GroupContainer` (UI). This is because the padding is necessary to have accurate size/position in the group model directly.

## Fixed

- Use `@key` when rendering the list of groups. Not using it caused big/weird render times.
- Groups not showing in Navigator/Overview.


## Diagrams [1.4.2] - 2020-12-30

## Added

- Locked nodes now have a `locked` class and their cursor is changed to `pointer`.

## Diagrams [1.4.1] - 2020-12-24

### Added

- `EnableVirtualization` option: whether to only render visible nodes or not.
- `RegisterModelComponent` overload that takes `Type`s as input for dynamic registrations.

## Diagrams [1.4.0] - 2020-12-12

### Added

- Two zoom related options, `Minimum` and `Maximum`, to clamp the zoom value.

### Changed

- **[BREAKING]** Grouped zoom related options into `DiagramZoomOptions`, available unnder `Options.Zoom`.
	- The option `AllowZooming` was renamed to `Enabled`.
	- The option `InverseZoom` was renamed to `Inverse`.

### Fixed

- The diagram canvas' container wasn't updated when the user scrolls ([#51](https://github.com/zHaytam/Blazor.Diagrams/issues/51)).

## Diagrams [1.3.0] - 2020-11-29

### Added
- Abstract `MovableModel`, which inherits from `SelectableModel` and represents models that can be move with the mouse (e.g. nodes and groups).
- Groups widget customization using `RegisterModelComponent`.
- `SizeChanged` event on `NodeModel`.

### Changed
- `SelectableModel` is now abstract.
- Groups:
  - Renamed model `Group` to `GroupModel`.
  - Rendered as a single entity (HTML div) with padding.
  - Movable and Selectable.
  - Selecting a node inside a group doesn't select the others anymore.
- **[BREAKING]** Renamed `DiagramManager.ChangePan` to `UpdatePan`.
- **[BREAKING]** Renamed `DiagramManager.ChangeZoom` to `SetZoom`.

### Fixed
- `ZoomToFit` wasn't unhiding hidden nodes.

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
