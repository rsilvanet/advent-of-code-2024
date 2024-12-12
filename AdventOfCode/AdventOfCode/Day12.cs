using System.Numerics;

public class Day12 : Day
{
    public override string Solve1() => ExploreRegions().Sum(region => region.Area * region.PerimeterSteps).ToString();

    public override string Solve2() => ExploreRegions().Sum(region => region.Area * region.CountPerimeterLines()).ToString();

    private IEnumerable<Region> ExploreRegions()
    {
        var unvisited = new HashSet<Vector2>(Map.Keys);

        while (unvisited.Any())
        {
            var start = unvisited.First();
            var queue = new Queue<Vector2>([start]);
            var region = new Region(Map[start]);

            while (queue.TryDequeue(out var position))
            {
                if (!unvisited.Contains(position))
                {
                    continue;
                }

                region.Expand(GetPerimeter(position));
                unvisited.Remove(position);

                foreach (var nextPosition in MatrixHelper.FourDirections.Select(d => position + d).Where(p => Map.IsInside(p) && Map[p] == Map[start]))
                {
                    queue.Enqueue(nextPosition);
                }
            }

            yield return region;
        }
    }

    private IEnumerable<(Vector2 Position, PerimeterType Type)> GetPerimeter(Vector2 position)
    {
        foreach (var direction in MatrixHelper.FourDirections.Select(d => (Position: position + d, Type: GetPerimeterType(d))))
        {
            if (IsPerimeter(position, direction.Position))
            {
                yield return direction;
            }
        }
    }

    private PerimeterType GetPerimeterType(Vector2 direction) => direction switch
    {
        _ when direction == MatrixHelper.Up => PerimeterType.HorizontalUpper,
        _ when direction == MatrixHelper.Down => PerimeterType.HorizontalBottom,
        _ when direction == MatrixHelper.Left => PerimeterType.VerticalLeft,
        _ when direction == MatrixHelper.Right => PerimeterType.VerticalRight,
        _ => throw new NotImplementedException()
    };

    private bool IsPerimeter(Vector2 initial, Vector2 current) => !Map.IsInside(current) || Map[current] != Map[initial];

    private class Region
    {
        public Region(char letter)
        {
            Letter = letter;
            FullPerimeter = Enumerable.Empty<(Vector2 Position, PerimeterType Type)>();
        }

        public char Letter { get; private set; }
        public int Area { get; private set; }
        public int PerimeterSteps { get; private set; }
        public IEnumerable<(Vector2 Position, PerimeterType Type)> FullPerimeter { get; private set; }

        public void Expand(IEnumerable<(Vector2 Position, PerimeterType Type)> perimeter)
        {
            Area++;
            PerimeterSteps += perimeter.Count();
            FullPerimeter = FullPerimeter.Concat(perimeter);
        }

        public int CountPerimeterLines()
        {
            var verticals = FullPerimeter
                .Where(x => x.Type is PerimeterType.VerticalLeft or PerimeterType.VerticalRight)
                .OrderBy(x => x.Type).ThenBy(x => x.Position.Y).ThenBy(x => x.Position.X);

            var horizontals = FullPerimeter
                .Where(x => x.Type is PerimeterType.HorizontalUpper or PerimeterType.HorizontalBottom)
                .OrderBy(x => x.Type).ThenBy(x => x.Position.X).ThenBy(x => x.Position.Y);

            return CountLines(verticals) + CountLines(horizontals);
        }

        private int CountLines(IEnumerable<(Vector2 Position, PerimeterType Type)> items)
        {
            var counter = 0;
            var currentLine = new List<(Vector2 Position, PerimeterType Type)>();

            foreach (var item in items)
            {
                if (currentLine.Count > 0 && IsAdjacent(currentLine.Last(), item))
                {
                    currentLine.Add(item);
                    continue;
                }

                currentLine = new List<(Vector2 Position, PerimeterType Type)> { item };
                counter++;
            }

            return counter;
        }

        private bool IsAdjacent((Vector2 Position, PerimeterType Type) item1, (Vector2 Position, PerimeterType Type) item2)
        {
            if (item1.Type is PerimeterType.VerticalLeft or PerimeterType.VerticalRight)
            {
                return item1.Type == item2.Type && item1.Position.Y == item2.Position.Y && Math.Abs(item1.Position.X - item2.Position.X) == 1;
            }

            return item1.Type == item2.Type && item1.Position.X == item2.Position.X && Math.Abs(item1.Position.Y - item2.Position.Y) == 1;
        }
    }

    private enum PerimeterType
    {
        VerticalLeft,
        VerticalRight,
        HorizontalUpper,
        HorizontalBottom,
    }

    public Day12()
    {
        Map = new Matrix(Input);
    }

    private Matrix Map { get; }
}
