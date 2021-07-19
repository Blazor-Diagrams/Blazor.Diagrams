# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Diagrams (2.1.3) - 2021-07-19

### Added

- Multiple demo projects by [TrevorDArcyEvans](https://github.com/TrevorDArcyEvans) showing:
	- Minimal project setup
	- Custom nodes & links
	- GraphShape layout algorithms

### Fixed

- Link labels not appearing (fixes #109).
- `ZoomToFit` throwing a `NullReferenceException` when one of the nodes doesn't have a size yet (fixes #118).

## Diagrams (2.1.2) - 2021-05-31

### Fixed

- Inability to interact with nodes after re-using ids on new node instances.

## Diagrams (2.1.1) - 2021-05-25

### Fixed

- Exception when navigating to the Options page (#95).
- Portless links not deleted when node is deleted (#96).

## Diagrams (2.1.0) - 2021-03-24

## Added

- `ScaleFactor` zoom option ([@joezearing](https://github.com/joezearing))
- Add `Constraints` options which gives you more control on what happens in the diagram:
	- `ShouldDeleteNode`: Whether a selected node should be deleted or not
	- `ShouldDeleteLink`: Whether a selected link should be deleted or not
	- `ShouldDeleteGroup`: Whether a selected group should be deleted or not
- `DeleteSelectionBehavior` unit tests

## Diagrams (2.0.0) - 2021-03-24

### Added

- `GroupUngrouped` event (not the greatest name 😄)
- Touch events `TouchStart`, `TouchMove` and `TouchEnd`
- Mobile support
	- Dragging new links from ports
	- Selecting models
	- Panning
	- Dragging movables
- `RemoveGroup` method to delete a group and all its children
- Pressing DEL on a selected group will now remove it
- Ability for links to be segmentable (by setting `Segmentable` to `true`)
	- Clicking a segmentable link will add a new vertex in that position
	- Vertices are movable models, which mean you can drag them to reposition them
	- Double clicking a vertex will delete it
- Link routers
  - Routers are functions that take into account a link's vertices, and decide whether to add more points along the path
  - They are specified using `Options.Links.DefaultRouter` or by setting `Router` in links
  - There are currently two routers, `Routers.Normal` and `Routers.Orthogonal`
  - You can provide your own router by simply respecting the delegate `Router`
- Link path generators
  - Path generators are functions that takes as input a link's route, and return a list of generated SVG paths (strings), as well as the position and angle of the source/target markers
  - They are specified using `Options.Links.DefaultPathGenerator` or by setting `PathGenerator` in links
  - There are currently two generators, `PathGenerators.Straight` and `PathGenerators.Smooth`
  - You can provide your own generator by simply respecting the delegate `PathGenerator`
- On-the-fly link markers
	- A link can either have a `SourceMarker` or `TargetMarker`
	- Link markers are SVG Paths with a known width (to calculate angle and where to cut the links)
	- Static markers are available (e.g. `LinkMarker.Arrow`) as well as utility methods to create known shapes
- `MouseClick` event, which only fires if the model (or canvas) in question didn't move, as opposed to MouseUp
- `data-X-id` HTML attributes to nodes, ports and links
- Abstract `BaseLinkModel`, `LinkModel` now inherits from it
	- All the parts of the library now refer to `BaseLinkModel` instead of `LinkModel` (e.g. PortModel.Links)
- `Color`, `SelectedColor` and `Width` properties to `LinkModel`
- Link labels
	- Can have custom models, using `RegisterModelComponent`
	- Due to the limitation of SVGs in Blazor, labels aren't interactive and must be created using `MarkupString`
	- They can have a specified distance
		- A number between 0 and 1: Position relative to the link's length
        - A positive number, greater than 1: Position away from the start
        - A negative number, less than 0: Position away from the end
    - They can have an offset (Point)
- Link snapping
	- While the user is dragging a link, it will try to find the closest port it can attach the link to
	- Options (in `DiagramOptions.Links`): `EnableSnapping` and `SnappingRadius`
- `GetBounds` extension method on `IEnumerable<NodeModel>`
- Expose `Padding` property in `GroupModel`
- `Factory` option to `Options.Links`
	- This is a delegate (`BaseLinkModel LinkFactory(DiagramManager diagram, PortModel sourcePort)`) to let you control how links are created
- `Groups` (of type `DiagramGroupOptions`) option to `DiagramOptions`
	- `Enabled` option controls whether users are allow to group nodes together or not
	- `KeyboardShortcut` (`Func<KeyboardEventArgs, bool>`) controls what keys need to be pressed to trigger the behavior
	- `Factory` lets you control how groups are created
- `GetRelativePoint` method to `Diagram`
	- This method gives the relative mouse point on the canvas
- `SelectionBoxWidget`
	- If used, holding SHIFT and clicking then dragging will show a selection box that selects all models intersecting with it
- Minified versions of all assets
- Observe document body mutations to update canvas container
- Ability to add/remove children after the group is created
- `Diagram` now also inherits `Model`
- Ability to suspend refreshes in `Diagram` by setting `Diagram.SuspendRefresh` to `true`
	- As the name suggests, this will stop the method `Refresh` from triggering the event `Changed`, which tries to re-render the UI
- `Batch(Action action)` in `Diagram`.
	- This is just a helper method that suspends refreshes, runs the action and then refreshes
	- This is now used internally in node/link layers to only trigger 1 UI re-render
- Add `FillColor` parameter to `NavigatorWidget`
- Ability to create links between two nodes directly, without needing ports
	- This depends on the shape of the nodes, which can be defined by providing a `ShapeDefiner` in `NodeModel`'s constructor
	- The default shape definer is `Shapes.Rectangle`
	- Currently, there is only the Rectangle and Ellipse (including Circle) shapes, others will be added later
	- This also works for groups, since groups are nodes
- Unit tests

### Changed

- Renamed `DiagramManager` to `Diagram`
- Remove need to specify `Name` in diagram's `CascadingValue`
- Separate Nodes/Links from DiagramManager, they are now layers (`Diagram.Nodes` and `Diagram.Links`)
- 	- Adding/Removing nodes or links is now done inside these layers (e.g. `Diagram.Nodes.Add`)
	- Added/Removed events are now inside these layers, not in the diagram (e.g. `Diagram.Links.Added`)
- `LinkAttached` event removed (`TargetPortChanged` is the alternative)
- It is now mandatory to add nodes to diagram before creating groups
- `SelectionChanged` event only contains the model now, use `Selected` property
- Removed `SelectedModels` in favor of `GetSelectedModels()`, this is because we don't hold a list of selected models anymore (unnecessary)
- Renamed `DiagramSubManager` to `Behavior`, it makes more sense
- `RegisterBehavior` now takes as an argument the behavior instance to add. No need to use `Activator.CreateInstance` for something like this, as it just slows things down
- Removed `LinkType` enum
- Removed `DefaultLinkType` link option
- Removed `DefaultLinkModel` link option
- Removed `GetNodesRect` method from DiagramManager (use `GetBounds`)
- Removed diagram dependency from `GroupModel` (was only using `GetNodesRect`)
- Renamed `GetRelativePoint` to `GetRelativeMousePoint`
	- This method gives the relative mouse point inside the diagram, taking into account the current pan & zoom
- `Widgets` are inside the `DiagramCanvas` now
	- This change is necessary so that widgets with absolute position have their relative parent be the canvas 
- Renamed `Navigator` widget to `NavigatorWidget`
- Allow empty groups
- Compare received JS sizes (width and height) with precision of 0.0001
	- In most cases, sizes retrieved from JS (especially with a zoom <> 1) can't be compared accurately (e.g. `80` and `79.9999975`). We fix this by comparing with a tolerance.
- Update ports dimensions only if Initialized is false
	- This avoids useless re-renders/JS calls when node is re-visible
- `NodeModel.Ports` is now a `IReadOnlyList<PortModel>`
- `BaseLinkModelExtensions` is now obsolete
- Moved everything in `Blazor.Diagrams.Core.Models.Core` to `Blazor.Diagrams.Core.Geometry`
	- This includes `Point`, `Size` and `Rectangle`
	- This is to better structure things in the project, the new `Geometry` namespace will contain many other things related to it
- Only render links when ports/nodes are initialized (position and/or size received)
	- This will avoid the weird flicker where links show at (0, 0) then move to the correct position

### Fixed

- Remove links when groups are removed
- Issue where links are clickable outside the visible stroke
	- `pointer-events` is now set to `visiblePainted` instead of `all`
- `MouseUp` event bubbles up from `PortModel` to `NodeModel`
- `Size` not taking into account zoom when nodes become visible again
- Only allow link creation using left mouse button
- JS errors in razor pages without a diagram
- CustomNodeWidget was movable from text input
	- All users who create custom nodes with HTML inputs should use `x:stopPropagation` on `mousedown`, `mousemove` and `mouseup` to prevent the node from being movable through inputs.
- Deleted nodes from groups would still show them
- `NavigatorWidget` not handling negative node positions
- Panning with right click. It is now disallowed
- PortRender not taking into account its parent's `RenderLayer`
  - Ports on SVG nodes will now render as `<g>` elements
- Useless refreshes when diagram `Container` values didn't change

## Diagrams [1.5.2] - 2021-01-18

### Fixed

- Missing MouseUp event on links.

## Diagrams [1.5.1] - 2021-01-09

### Added

- `AddGroup`: add an instance of a group to the diagram.
- Custom group documentation/demo.

### Fixed

- Clicking the canvas in the Events demo throws an exception.

## Diagrams [1.5.0] - 2021-01-05

### Added

- The ability to have ports on groups.
- **EXPERIMENTAL/INCOMPLETE** Nested groups. Since `GroupModel` now inherits `NodeMode`, it became possible to have nested groups, but there are still problems with the order of links between groups.
- A `Class` parameter to `GroupContainer`.

### Changed

- Only rerender groups when necessary.
- Receiving the same size from `ResizeObserver` doesn't trigger a rerender anymore.
- Avoid rerendering ports twice to update positions.
- Avoid rerendering ports when their parent node is moving.
- Padding is now handled in `GroupModel` instead of `GroupContainer` (UI). This is because the padding is necessary to have accurate size/position in the group model directly.

### Fixed

- Use `@key` when rendering the list of groups. Not using it caused big/weird render times.
- Groups not showing in Navigator/Overview.

## Diagrams [1.4.2] - 2020-12-30

### Added

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
