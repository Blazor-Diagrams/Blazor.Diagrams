# Blazor.Diagrams

Blazor.Diagrams is a fully customizable and extensible all-purpose diagrams library for Blazor (both Server Side and WASM). It was inspired by the popular React library [react-diagrams](https://github.com/projectstorm/react-diagrams).  

Blazor.Diagrams can be used to make advanced diagrams with a custom design. Even the behavior of the library is "hackable" and can be changed to suit your needs. The purpose of this library is to give as much functionality as possible with as little JavaScript Interop as possible.  

Blazor.Diagrams is very code/OOP oriented for now, this has a lot of benefits, for example if you want to (de)serialize models or even make an engine that runs at runtime (visual programming).

This project is currently in Alpha, all issues/MRs are welcome!

| NuGet Package          | Version                                                                                                                      | Download                                                                                                                      |
| ---------------------- | ---------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------- |
| Z.Blazor.Diagrams.Core | [![NuGet](https://img.shields.io/nuget/v/Z.Blazor.Diagrams.Core.svg)](https://www.nuget.org/packages/Z.Blazor.Diagrams.Core) | [![Nuget](https://img.shields.io/nuget/dt/Z.Blazor.Diagrams.Core.svg)](https://www.nuget.org/packages/Z.Blazor.Diagrams.Core) |
| Z.Blazor.Diagrams      | [![NuGet](https://img.shields.io/nuget/v/Z.Blazor.Diagrams.svg)](https://www.nuget.org/packages/Z.Blazor.Diagrams)           | [![Nuget](https://img.shields.io/nuget/dt/Z.Blazor.Diagrams.svg)](https://www.nuget.org/packages/Z.Blazor.Diagrams)           |


| Badges   |                                                                                                                                    |
| -------- | ---------------------------------------------------------------------------------------------------------------------------------- |
| Activity | [![GitHub](https://img.shields.io/github/last-commit/zHaytam/Blazor.Diagrams/develop)](https://github.com/zHaytam/Blazor.Diagrams) |
| License  | [![GitHub](https://img.shields.io/github/license/zHaytam/Blazor.Diagrams.svg)](https://github.com/zHaytam/Blazor.Diagrams)         |

## Getting Started

You can get started very easily & quickly using:

- [Documentation](https://blazor-diagrams.zhaytam.com/)
- [Quick Start](https://blazor-diagrams.zhaytam.com/quickstart)
- [Demos](https://blazor-diagrams.zhaytam.com/demos/simple)

There are a lot of things missing in the documentation, such as customization. They will be added as things progress. In the meantime, the demos should show you how to do most things.

## Functionalities

- Panning/Zooming
- Ports & Links (no free links for now)
- Locking mechanism
- Custom nodes/links
- SVG layer for links and DIV layer for nodes for maximum customizability
- Replaceable behaviors (e.g. link dragging, model deletion, ...)

## Preview

![](https://i.imgur.com/YhcjaTn.png)

## Roadmap

- [ ] Groups
- [ ] Razor-oriented diagram creation
- [ ] Auto layout (might add a lot of js interop)
- [ ] Zoom to fit (might add a lot of js interop too)
- [ ] History (undo/redo)
- [ ] Preview/Navigator on the bottom-right 
- [ ] Avoid rendering models outside of screen view (might add a lot of js interop)
- [ ] Send to front/back (limited since there are 2 layers)
- [ ] Free links, no need for ports (useful in simple diagram scenarios, like a flowchart)
- [ ] Ability to add nodes in the SVG layer (useful in simple diagram scenarios, like a flowchart)
- [ ] A set of common shapes (depends on the above feature)

## Feedback

If you find a bug or you want to see a functionality in this library, feel free to open an issue in the repository!  
Of course, PRs are very welcome.
