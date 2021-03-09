using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

// Implementation taken from the JS version: https://gist.github.com/menendezpoo/4a8894c152383b9d7a870c24a04447e4
// Todo: Make it more c#, Benchmark A* vs Dijkstra, Add more options
namespace Blazor.Diagrams.Core
{
    public static partial class Routers
    {
        public static Point[] Orthogonal(Diagram _, BaseLinkModel link)
        {
            if (link.TargetPort == null || link.SourcePort.Parent.Size == null || link.TargetPort.Parent.Size == null)
                return Normal(_, link);

            var shapeMargin = 10;
            var globalBoundsMargin = 50;
            var spots = new List<Point>();
            var verticals = new List<double>();
            var horizontals = new List<double>();
            var sideA = link.SourcePort.Alignment;
            var sideAVertical = IsVerticalSide(sideA);
            var sideB = link.TargetPort.Alignment;
            var sideBVertical = IsVerticalSide(sideB);
            var originA = GetPortPositionBasedOnAlignment(link.SourcePort);
            var originB = GetPortPositionBasedOnAlignment(link.TargetPort);
            var shapeA = link.SourcePort.Parent.GetBounds(includePorts: true);
            var shapeB = link.TargetPort.Parent.GetBounds(includePorts: true);
            var inflatedA = shapeA.Inflate(shapeMargin, shapeMargin);
            var inflatedB = shapeB.Inflate(shapeMargin, shapeMargin);

            if (inflatedA.Intersects(inflatedB))
            {
                shapeMargin = 0;
                inflatedA = shapeA;
                inflatedB = shapeB;
            }

            // Curated bounds to stick to
            var bounds = inflatedA.Union(inflatedB).Inflate(globalBoundsMargin, globalBoundsMargin);

            // Add edges to rulers
            verticals.Add(inflatedA.Left);
            verticals.Add(inflatedA.Right);
            horizontals.Add(inflatedA.Top);
            horizontals.Add(inflatedA.Bottom);
            verticals.Add(inflatedB.Left);
            verticals.Add(inflatedB.Right);
            horizontals.Add(inflatedB.Top);
            horizontals.Add(inflatedB.Bottom);

            // Rulers at origins of shapes
            (sideAVertical ? verticals : horizontals).Add(sideAVertical ? originA.X : originA.Y);
            (sideBVertical ? verticals : horizontals).Add(sideBVertical ? originB.X : originB.Y);

            // Points of shape antennas
            spots.Add(GetOriginSpot(originA, sideA, shapeMargin));
            spots.Add(GetOriginSpot(originB, sideB, shapeMargin));

            // Sort rulers
            verticals.Sort();
            horizontals.Sort();

            // Create grid
            var grid = RulersToGrid(verticals, horizontals, bounds);
            var gridPoints = GridToSpots(grid, new[] { inflatedA, inflatedB });

            // Add to spots
            spots.AddRange(gridPoints);

            // Create graph
            var graph = CreateGraph(spots);

            // Origin and destination by extruding antennas
            var origin = ExtrudeCp(originA, shapeMargin, sideA);
            var destination = ExtrudeCp(originB, shapeMargin, sideB);

            var start = originA;
            var end = originB;

            var path = ShortestPath(graph, origin, destination);
            if (path.Length > 0)
            {
                var result = new List<Point>();
                result.Add(start);
                result.AddRange(SimplifyPath(path));
                result.Add(end);
                return result.ToArray();
            }
            else
            {
                return Normal(_, link);
            }
        }

        private static Point GetOriginSpot(Point p, PortAlignment alignment, double shapeMargin)
        {
            return alignment switch
            {
                PortAlignment.Top => p.Add(0, -shapeMargin),
                PortAlignment.Right => p.Add(shapeMargin, 0),
                PortAlignment.Bottom => p.Add(0, shapeMargin),
                PortAlignment.Left => p.Add(-shapeMargin, 0),
                _ => throw new NotImplementedException()
            };
        }

        private static bool IsVerticalSide(PortAlignment alignment)
            => alignment == PortAlignment.Top || alignment == PortAlignment.Bottom; // Add others

        private static Grid RulersToGrid(List<double> verticals, List<double> horizontals, Rectangle bounds)
        {
            var result = new Grid();
            verticals.Sort();
            horizontals.Sort();

            var lastX = bounds.Left;
            var lastY = bounds.Top;
            var column = 0;
            var row = 0;

            foreach (var y in horizontals)
            {
                foreach (var x in verticals)
                {
                    result.Set(row, column++, new Rectangle(lastX, lastY, x, y));
                    lastX = x;
                }

                // Last cell of the row
                result.Set(row, column, new Rectangle(lastX, lastY, bounds.Right, y));
                lastX = bounds.Left;
                lastY = y;
                column = 0;
                row++;
            }

            lastX = bounds.Left;

            // Last fow of cells
            foreach (var x in verticals)
            {
                result.Set(row, column++, new Rectangle(lastX, lastY, x, bounds.Bottom));
                lastX = x;
            }

            // Last cell of last row
            result.Set(row, column, new Rectangle(lastX, lastY, bounds.Right, bounds.Bottom));
            return result;
        }

        private static List<Point> GridToSpots(Grid grid, Rectangle[] obstacles)
        {
            bool obstacleCollision(Point p) => obstacles.Where(o => o.ContainsPoint(p)).Any();

            var gridPoints = new List<Point>();
            foreach (var (row, data) in grid.Data)
            {

                var firstRow = row == 0;
                var lastRow = row == grid.Rows - 1;

                foreach (var (col, r) in data)
                {

                    var firstCol = col == 0;
                    var lastCol = col == grid.Columns - 1;
                    var nw = firstCol && firstRow;
                    var ne = firstRow && lastCol;
                    var se = lastRow && lastCol;
                    var sw = lastRow && firstCol;

                    if (nw || ne || se || sw)
                    {
                        gridPoints.Add(r.NorthWest);
                        gridPoints.Add(r.NorthEast);
                        gridPoints.Add(r.SouthWest);
                        gridPoints.Add(r.SouthEast);
                    }
                    else if (firstRow)
                    {
                        gridPoints.Add(r.NorthWest);
                        gridPoints.Add(r.North);
                        gridPoints.Add(r.NorthEast);
                    }
                    else if (lastRow)
                    {
                        gridPoints.Add(r.SouthEast);
                        gridPoints.Add(r.South);
                        gridPoints.Add(r.SouthWest);
                    }
                    else if (firstCol)
                    {
                        gridPoints.Add(r.NorthWest);
                        gridPoints.Add(r.West);
                        gridPoints.Add(r.SouthWest);
                    }
                    else if (lastCol)
                    {
                        gridPoints.Add(r.NorthEast);
                        gridPoints.Add(r.East);
                        gridPoints.Add(r.SouthEast);
                    }
                    else
                    {
                        gridPoints.Add(r.NorthWest);
                        gridPoints.Add(r.North);
                        gridPoints.Add(r.NorthEast);
                        gridPoints.Add(r.East);
                        gridPoints.Add(r.SouthEast);
                        gridPoints.Add(r.South);
                        gridPoints.Add(r.SouthWest);
                        gridPoints.Add(r.West);
                        gridPoints.Add(r.Center);
                    }
                }
            }

            // Reduce repeated points and filter out those who touch shapes
            return ReducePoints(gridPoints).Where(p => !obstacleCollision(p)).ToList();
        }

        private static IEnumerable<Point> ReducePoints(List<Point> points)
        {
            var map = new Dictionary<double, List<double>>();
            foreach (var p in points)
            {
                (var x, var y) = p;
                if (!map.ContainsKey(y)) map.Add(y, new List<double>());
                var arr = map[y];

                if (!arr.Contains(x)) arr.Add(x);
            }

            foreach (var (y, xs) in map)
            {
                foreach (var x in xs)
                {
                    yield return new Point(x, y);
                }
            }
        }

        private static PointGraph CreateGraph(List<Point> spots)
        {
            var hotXs = new List<double>();
            var hotYs = new List<double>();
            var graph = new PointGraph();

            spots.ForEach(p =>
            {
                (var x, var y) = p;
                if (!hotXs.Contains(x)) hotXs.Add(x);
                if (!hotYs.Contains(y)) hotYs.Add(y);
                graph.Add(p);
            });

            hotXs.Sort();
            hotYs.Sort();

            for (var i = 0; i < hotYs.Count; i++)
            {
                for (var j = 0; j < hotXs.Count; j++)
                {
                    var b = new Point(hotXs[j], hotYs[i]);
                    if (!graph.Has(b)) continue;

                    if (j > 0)
                    {
                        var a = new Point(hotXs[j - 1], hotYs[i]);

                        if (graph.Has(a))
                        {
                            graph.Connect(a, b);
                            graph.Connect(b, a);
                        }
                    }

                    if (i > 0)
                    {
                        var a = new Point(hotXs[j], hotYs[i - 1]);

                        if (graph.Has(a))
                        {
                            graph.Connect(a, b);
                            graph.Connect(b, a);
                        }
                    }
                }
            }

            return graph;
        }

        private static Point ExtrudeCp(Point p, double margin, PortAlignment alignment)
        {
            return alignment switch
            {
                PortAlignment.Top => p.Add(0, -margin),
                PortAlignment.Right => p.Add(margin, 0),
                PortAlignment.Bottom => p.Add(0, margin),
                PortAlignment.Left => p.Add(-margin, 0),
                _ => throw new NotImplementedException(),
            };
        }

        private static Point[] ShortestPath(PointGraph graph, Point origin, Point destination)
        {
            var originNode = graph.Get(origin);
            var destinationNode = graph.Get(destination);

            if (originNode == null || destinationNode == null)
                throw new Exception("Origin node or Destination node not found");

            graph.CalculateShortestPathFromSource(graph, originNode);
            return destinationNode.ShortestPath.Select(n => n.Data).ToArray();
        }

        private static Point[] SimplifyPath(Point[] points)
        {
            if (points.Length <= 2)
            {
                return points;
            }

            var r = new List<Point>() { points[0] };
            for (var i = 1; i < points.Length; i++)
            {
                var cur = points[i];
                if (i == (points.Length - 1))
                {
                    r.Add(cur);
                    break;
                }

                var prev = points[i - 1];
                var next = points[i + 1];
                var bend = GetBend(prev, cur, next);

                if (bend != "none")
                {
                    r.Add(cur);
                }
            }

            return r.ToArray();
        }

        private static string GetBend(Point a, Point b, Point c)
        {
            var equalX = a.X == b.X && b.X == c.X;
            var equalY = a.Y == b.Y && b.Y == c.Y;
            var segment1Horizontal = a.Y == b.Y;
            var segment1Vertical = a.X == b.X;
            var segment2Horizontal = b.Y == c.Y;
            var segment2Vertical = b.X == c.X;

            if (equalX || equalY)
            {
                return "none";
            }

            if (
                !(segment1Vertical || segment1Horizontal) ||
                !(segment2Vertical || segment2Horizontal)
            )
            {
                return "unknown";
            }

            if (segment1Horizontal && segment2Vertical)
            {
                return c.Y > b.Y ? "s" : "n";

            }
            else if (segment1Vertical && segment2Horizontal)
            {
                return c.X > b.X ? "e" : "w";
            }

            throw new Exception("Nope");
        }
    }

    class Grid
    {
        public Grid()
        {
            Data = new Dictionary<double, Dictionary<double, Rectangle>>();
        }

        public Dictionary<double, Dictionary<double, Rectangle>> Data { get; }
        public double Rows { get; private set; }
        public double Columns { get; private set; }

        public void Set(double row, double column, Rectangle rectangle)
        {
            Rows = Math.Max(Rows, row + 1);
            Columns = Math.Max(Columns, column + 1);

            if (!Data.ContainsKey(row))
            {
                Data.Add(row, new Dictionary<double, Rectangle>());
            }

            Data[row].Add(column, rectangle);
        }

        public Rectangle? Get(double row, double column)
        {
            if (!Data.ContainsKey(row))
                return null;

            if (!Data[row].ContainsKey(column))
                return null;

            return Data[row][column];
        }

        public Rectangle[] Rectangles() => Data.SelectMany(r => r.Value.Values).ToArray();
    }

    class PointGraph
    {
        public readonly Dictionary<string, Dictionary<string, PointNode>> _index = new Dictionary<string, Dictionary<string, PointNode>>();

        public void Add(Point p)
        {
            (var x, var y) = p;
            var xs = x.ToInvariantString();
            var ys = y.ToInvariantString();

            if (!_index.ContainsKey(xs))
                _index.Add(xs, new Dictionary<string, PointNode>());

            if (!_index[xs].ContainsKey(ys))
                _index[xs].Add(ys, new PointNode(p));
        }

        private PointNode GetLowestDistanceNode(HashSet<PointNode> unsettledNodes)
        {
            PointNode? lowestDistanceNode = null;
            var lowestDistance = double.MaxValue;
            foreach (var node in unsettledNodes)
            {
                var nodeDistance = node.Distance;
                if (nodeDistance < lowestDistance)
                {
                    lowestDistance = nodeDistance;
                    lowestDistanceNode = node;
                }
            }

            return lowestDistanceNode!;
        }

        public PointGraph CalculateShortestPathFromSource(PointGraph graph, PointNode source)
        {
            source.Distance = 0;
            var settledNodes = new HashSet<PointNode>();
            var unsettledNodes = new HashSet<PointNode>
            {
                source
            };

            while (unsettledNodes.Count != 0)
            {
                var currentNode = GetLowestDistanceNode(unsettledNodes);
                unsettledNodes.Remove(currentNode);

                foreach ((var adjacentNode, var edgeWeight) in currentNode.AdjacentNodes)
                {
                    if (!settledNodes.Contains(adjacentNode))
                    {
                        CalculateMinimumDistance(adjacentNode, edgeWeight, currentNode);
                        unsettledNodes.Add(adjacentNode);
                    }

                }
                settledNodes.Add(currentNode);
            }

            return graph;
        }

        private void CalculateMinimumDistance(PointNode evaluationNode, double edgeWeight, PointNode sourceNode)
        {
            var sourceDistance = sourceNode.Distance;
            var comingDirection = InferPathDirection(sourceNode);
            var goingDirection = DirectionOfNodes(sourceNode, evaluationNode);
            var changingDirection = comingDirection != null && goingDirection != null && comingDirection != goingDirection;
            var extraWeigh = changingDirection ? Math.Pow(edgeWeight + 1, 2) : 0;

            if (sourceDistance + edgeWeight + extraWeigh < evaluationNode.Distance)
            {
                evaluationNode.Distance = sourceDistance + edgeWeight + extraWeigh;
                var shortestPath = new List<PointNode>();
                shortestPath.AddRange(sourceNode.ShortestPath);
                shortestPath.Add(sourceNode);
                evaluationNode.ShortestPath = shortestPath;
            }
        }

        private char? DirectionOf(Point a, Point b)
        {
            if (a.X == b.X) return 'h';
            else if (a.Y == b.Y) return 'v';
            return null;
        }

        private char? DirectionOfNodes(PointNode a, PointNode b) => DirectionOf(a.Data, b.Data);

        private char? InferPathDirection(PointNode node)
        {
            if (node.ShortestPath.Count == 0)
                return null;

            return DirectionOfNodes(node.ShortestPath[node.ShortestPath.Count - 1], node);
        }

        public void Connect(Point a, Point b)
        {
            var nodeA = Get(a);
            var nodeB = Get(b);

            if (nodeA == null || nodeB == null)
                return;

            nodeA.AdjacentNodes.Add(nodeB, a.DistanceTo(b));
        }

        public bool Has(Point p)
        {
            (var x, var y) = p;
            var xs = x.ToInvariantString();
            var ys = y.ToInvariantString();
            return _index.ContainsKey(xs) && _index[xs].ContainsKey(ys);
        }

        public PointNode? Get(Point p)
        {
            (var x, var y) = p;
            var xs = x.ToInvariantString();
            var ys = y.ToInvariantString();

            if (_index.ContainsKey(xs) && _index[xs].ContainsKey(ys))
                return _index[xs][ys];

            return null;
        }
    }

    class PointNode
    {
        public PointNode(Point data)
        {
            Data = data;
        }

        public Point Data { get; }
        public double Distance { get; set; } = double.MaxValue;
        public List<PointNode> ShortestPath { get; set; } = new List<PointNode>();
        public Dictionary<PointNode, double> AdjacentNodes { get; set; } = new Dictionary<PointNode, double>();
    }
}
