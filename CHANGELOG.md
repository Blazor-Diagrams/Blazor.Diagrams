
# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Diagram (3.0.0) - 2024-02-24

### Added

- Support for .NET 8 (thanks to @[SebastianWachsmuth](https://github.com/SebastianWachsmuth))

### Fixed

- Link labels not showing in Firefox
- NRE in the DemoSite's Options page (thanks to @[robertmclaws](https://github.com/robertmclaws))

## Diagrams (3.0.1) - 2023-10-27

### Added

- `Route` property to `BaseLinkModel` to hold the result of the executed router

### Fixed

- Constraints not checked when using `RemoveControl` (thanks to @[K0369](https://github.com/K0369))
- NRE Exception on the landing page demo when dragging a new link and not linking it to something (thanks to @[K0369](https://github.com/K0369))
- `LinkVertexWidgetTests` failing on cultures that are not using a dot as decimal separator (thanks to @[K0369](https://github.com/K0369))
- NRE exception in the Diagram Demo project (thanks to @[Suraj0500](https://github.com/Suraj0500))

## Diagrams (3.0.0) - 2023-08-14

Finally, the new documentation website is here!  
Please don't hesitate to create issues for any problems or improvements.  
PS: I suck at design.

### Added

- `AddLabel` method to links to easily create `LinkLabelModel`
- `AddVertex` method to links to easily create `LinkVertexModel`
- `ControlledSize` property to nodes. If `true`, the node will not be registered in the `ResizeObserver` (saves a JS call).
- `autoSize` argument to `SvgGroupModel` constructor

### Changed

- Renamed `Point.Substract` to `Subtract` (duh)
- Avoid rendering link selection helper on dragged link

### Fixed

- `SmoothPathGenerator` not working with `LinkAnchor`
- Mouse overlapping dragged link
- Useless Console Logs from `GroupModel` are now removed
- JS exception in `(un)oberve` methods when the element doesn't exist anymore

## Diagrams (3.0.0-beta.6) - 2023-05-09

### Added

- `Style` parameter to `PortRenderer`
- `TargetAttached` to links, which triggers when a dragged link attaches to a target
	- If port snapping is enabled, it will trigger only once when you let go of the mouse
- `SuspendSorting` to `Diagram` in order to suspend sorting models in each `OrderChanged`
	- If you know what you're doing, you could save some processing and avoid sorting everytime
- `RefreshOrders` to be called after unsuspending sorting in order to sort the models again and refresh the diagram

### Changed

- `BaseLayer.Add` now returns the specific type given to it in argument
- **[BREAKING]** CSS classes are now prefixed with `diagram-` to avoid clashes with other libraries
	- `diagram-group`, `diagram-node`, `diagram-link`, `diagram-port`, `diagram-link-label`, `diagram-link-vertex`, `diagram-control`

### Fixed

- Portless links in children not refreshing when moving the parent group
- Link's `GetBounds` not returning a valid box
- Port snapping choosing the first port in radius rather than the closest one
- Remove `Console.WriteLine` from `KeyboardShortcutsBehavior`
- Diagram overwriting `Order` when it's not zero (zero being the default int value, which we now consider as not set)

## Diagrams (3.0.0-beta.5) - 2022-11-23

### Added

- `AdditionalSvg` option to `DiagramCanvas` in order to render any exatra SVG content you want
- `AdditionalHtml` option to `DiagramCanvas` in order to render any exatra SVG content you want
- `DistanceTo` overload method to `Point` that takes x and y
- `MoveAlongLine` method to `Point`
- `FullPath` to `PathGeneratorResult` to represent the full path without cuts
- Fallback router to Orthogonal router
- Margin options to `OrthogonalRouter`
- `radius` option to `StraightPathGenerator` in order to generate rounded bends
- Support for custom vertices
- `AutoSize` option to groups to control whether moving children resizes the group

### Changed

- All routers are now classes instead of functions, they inherit from the new abstract class `Router`
- All path generators are now classes instead of functions, they inherit from the new abstract class `PathGenerator`
- Optimize Orthogonal router by using custom A* (x5 improvement)

### Removed

- `Router` delegate
- `PathGenerator` delegate

## Diagrams (3.0.0-beta.4) - 2022-10-16

### Added

- Initial version of Ordering!
	- Nodes, groups and links can now be ordered using the new `Order` property or `SendToFront/Back` methods
	- `Diagram.OrderedSelectables` returns the ordered selectables/models
	- `DiagramCanvas` now uses this new property to render everything
- `GridSnapToCenter` option in order to snap nodes from their center instead of their top/left position (thanks to @[Jeremy Vance](https://github.com/240026763))
- More unit tests

### Changed

- `Groups` is not a list of groups anymore, but a layer instead (just like `Nodes` and `Links`)

### Fixed

- Deleting a group doesn't delete links attached to it
- Deleting a group inside of a group doesn't refresh the parent group
- Links not refreshing when a group's dimensions are updated directly (e.g. deleting a child)
- Layers causing more refreshes than intended

### Removed

- All group-related methods and events from `Diagram`, please use the new layer from now on

## Diagrams (3.0.0-beta.3) - 2022-09-18

### Added

- Support for `LinkFactory` to return null in order to not create an ongoing link
- Support for free links (no source/target required)
- `PositionAnchor` which reprensents a simple plain position (mutable)
- `ArrowHeadControl` to control a link's Source/Target on the fly
- `attached` css class to attached links

### Changed

- Replace `OngoingPosition` with the new `PositionAnchor`
	- `BaseLinkModel.Target` will never be null anymore. An ongoing link will have a position anchor as the target
- `Links.Factory` signature now takes the diagram, source (model) and the target anchor
- Move `DynamicAnchor` back to `Anchors` namespace and seal all `Anchor` classes

### Fixed

- Links attached to links not refreshing when the others are
- `LinkPathPositionProvider` not working with maxlength ratios
- Deleting a link not deleting the links attached to it

### Removed

- `PositionProvider` argument from `ExecutableControl` for more freedom
- `Id` and `Refresh` from `ILinkable`
- Unused `Offset` from `Anchor` and make `Model` nullable

## Diagrams (3.0.0-beta.2) - 2022-09-11

### Added

- `Moved` event to Movables
- `Visible` property and `VisbilityChanged` event to models
- `Options.Virtualization` (of type `[Diagram]VirtualizationOptions`) for virtualization options
- `PointerEnter/Leave` events for groups as well 
- **Experimental Link to Link** (using `LinkAnchor`) 

### Changed

- Rename `RegisterModelComponent` to `RegisterComponent`
- Rename `GetComponentForModel` to `GetComponent`
- Virtualization is now handled by a behavior instead of NodeRender
  - This means that it works for almost all models (nodes, groups and links)
- Render link labels without foreignObject in widget nor MarkupString (Thank you .NET 6)
- Custom link labels only need to contain relevant content, they don't need to handle positioning anymore 

### Removed

- `EnableVirtualization` option (see added alternative)

## Diagrams (3.0.0-beta.1) - 2022-09-04

.NET 6!

A lot of things changed in this version, a lot of breaking changes were introduced but I believe it was necessary.   
Many changes were required to make everything clearer, customizable and cleaner (code wise).  
I'm aiming to completely decouple the Core library from the UI, because I'm thinking of giving MAUI Diagrams a try very soon!

### Added

- `BlazorDiagram` class (inherits `Diagram`) to the blazor package to replace the old Core one
- `BlazorDiagramOptions` that inherit from the other diagram options to add Blazor (UI) specific options
- `Blazor.Diagrams.Models.SvgNodeModel` class to represent a node that needs to be rendered in the SVG layer
- `GetBehavior<T>` method to `Diagram` in order to retrieve a registered behavior
- `KeyboardShortcutsBehavior` class which handles keyboard shortcuts/actions:
	- `SetShortcut`: sets an action (`Func<Diagrambase, ValueTask>`) to be executed whenever the specified combination is pressed
	- `RemoveShortcut`: removes a defined action (if it exists)
- `KeyboardShortcutsDefaults` containing the default shortcuts that were deleted (`DeleteSelection` and `Grouping`)
- Anchors functionality:
	- An Anchor determines where on an element the link will connect
	- Instead of links requiring the source and target to be either both nodes or both ports, there is now only one `Source` and `Target` of type `Anchor`
	- This lets the link not worry about the details of from/to where its going, as long as the anchor provides it with its position when asked for
	- Current implementations:
		- `SinglePortAnchor`: Specifies that the connection point is a specific port (supports shape & alignment)
		- `ShapeIntersectionAnchor`: Specifies that the connection point is the intersection of a line with the node's shape
		- `DynamicAnchor`: Specifies that the connection point is one of the given positions (closest)
- Virtual `IShape GetShape()` method on nodes (default `Rectangle`) and ports (default `Circle`) 
- `Options.LinksLayerOrder` to indicate the order of the links layer (svg for blazor)
- `Options.NodesLayerOrder` to indicate the order of the nodes layer (html for blazor)
- Support for SVG groups that also represent children as a hierarchy (`SvgGroupModel`)
- Node renderer will now append, in addition to `node locked`, the classes `selected grouped`
- `IHasBounds` and `IHasShape` interfaces to both nodes and ports 
- `IPositionProvider` to encapsulate how certain positions are calculated given a model
  - They are used for dynamic anchors and controls for now
  - `BoundsBasedPositionProvider` returns the position based on the bounds of the model (e.g. (0.5, 0.5) would be the center)
  - `ShapeAnglePositionProvider` returns the position as the point at the angle of the shape
  - `LinkPathPositionProvider` returns the position based on the link's path (`getPointAtLength`)
- Links have a reference to `Diagram` now
- `PointerEnter` and `PointerLeave` events for nodes and links for now 
- `GeneratedPathResult` and `Paths` to `BaseLinkModel` to always have access to the actual paths
- `Controls` feature (beta):
  - They are things that can show up on top of nodes/links and can even be clicked to be executed
  - Their UI is also picked up from the registered components 
  - `Control` designates a control that has a position and will be rendered if visible
  - `ExecutableControl` designates a control that has a position and will be executed when pressed (PointerDown event)
  - Default controls for now are:
    - `BoundaryControl` shows the model's boundary
    - `RemoveControl` shows a button that when clicked, removes the model from the diagram
    - `DragNewLink` shows a button that when clicked, starts a new link dragging from that node
- `GridWidget` a background grid that moves with the diagram instead of being fixed like in the Snap to grid example 
- More unit tests
	
### Changed

- Core package changes:
	- Web dependency was removed from the Core package
      - `Diagram` is now abstract
      - These changes were done to decouple the core from the rendering, in the future we might have a MAUI renderer
- `Diagram.GetComponentForModel` now accepts a `checkSubclasses` argument (default `true`)
- Constraints now must return a `ValueTask<bool>` instead of a simple `bool`
- Renamed `AllLinks` to `PortLinks` for more clarity on which links, since `Links` contains the others
- Dragging links from ports will now follow the mouse at the same pace minus 5 pixels so that it doesn't go on top of the link it self or other ports
- How groups are rendered
	- `GroupRenderer` will take care of rendering the group with the appropriate style and classes
	- Only `GroupNodes` is required, `GroupLinks` was deleted because all links are shown in the svg layer (with appropriate order)
- `Diagram.AddGroup` will now return the added group
- All `Mouse` events have been converted to `Pointer` events
- `PathGenerator` now return `SvgPath` instead of just strings 
- `NavigatorWidget` was rewritten to be faster, lighter, WORKING and customizable
  - It now can also take the shape of the nodes into account (rect and ellipse for now) 

### Fixed

- Virtualization throwing a JSException (#155)
- Ports not rendering correctly because of the missing `@key` (#220)
- Link not refreshing when a new vertex is created, which was showing out of link 

### Removed

- `DefaultNodeComponent` and `DefaultLinkComponent` options (see `GetComponentForModel` changes)
- `RenderLayer` from the Core package and all its usage
- `DeleteSelectionBehavior` since there is a new keyboard shortcuts system
- `GroupingBehavior` since there is a new keyboard shortcuts system
- `BaseLinkModelExtensions` since it was Obselete
- Unnecessary port refreshes when dragging a link ends or when link snapping
- `ShapeDefiner` delegate and constructor arguments on nodes since delegates can't be serialized
- `TouchX` events

## Diagrams (2.1.6) - 2021-10-31

### Fixed

- `ZoomBehavior` using new zoom before Clamp to set Pan. (fixes #141)
- `PanChanged` not triggering when zooming with the mouse wheel.
- Zoom value decreasing when the mouse wheel delta is zero.
- Ports aren't refreshed when links are added in `OnInitializedAsync`. (fixes #111)

## Diagrams (2.1.5) - 2021-08-30

### Fixed

- Links not being removed from the node after they have been removed from the Links layer. (fixes #136)
- A regression in `ZoomToFit`. (fixes #138)

## Diagrams (2.1.4) - 2021-08-29

### Added

- `MouseDoubleClick` (500ms interval) event in `Diagram`.
- `GetScreenPoint` in `Diagram` in order to get the screen points from a diagram point (e.g. node position).
- `Title` property in `NodeModel`, used by the default node widget.

### Fixed

- `ZoomToFit` not triggering `ZoomChanged` event.
- `SourceNode` and `TargetNode` not being set in `BaseLinkModel` when the ports change.

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
