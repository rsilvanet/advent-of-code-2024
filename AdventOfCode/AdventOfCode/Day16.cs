using System.Numerics;

public class Day16 : Day
{
    public override string Solve1() => Iterate(out _).ToString();

    public override string Solve2()
    {
        var lowest = Iterate(out var paths);
        var lowestPaths = paths.Where(x => x.Score == lowest);

        return lowestPaths.SelectMany(x => x.Path).Distinct().Count().ToString();
    }

    private int Iterate(out List<(IEnumerable<Vector2> Path, int Score)> paths)
    {
        var queue = new Queue<(Vector2 Position, Vector2 Direction, int Points, IEnumerable<Vector2> Path)>([(Start, MatrixHelper.Right, 0, [Start])]);
        var visited = new Dictionary<(Vector2, Vector2), int>();
        var lowest = int.MaxValue;

        paths = new List<(IEnumerable<Vector2>, int)>();

        while (queue.TryDequeue(out var item))
        {
            if (visited.TryGetValue((item.Position, item.Direction), out var cached))
            {
                visited[(item.Position, item.Direction)] = Math.Min(cached, item.Points);

                if (cached < item.Points)
                {
                    continue;
                }
            }
            else
            {
                visited[(item.Position, item.Direction)] = item.Points;
            }

            if (item.Position == End)
            {
                paths.Add((item.Path, item.Points));
                lowest = Math.Min(item.Points, lowest);
            }

            if (!Walls.Contains(item.Position + item.Direction.TurnRight()))
            {
                queue.Enqueue((item.Position, item.Direction.TurnRight(), item.Points + 1000, item.Path));
            }

            if (!Walls.Contains(item.Position + item.Direction.TurnLeft()))
            {
                queue.Enqueue((item.Position, item.Direction.TurnLeft(), item.Points + 1000, item.Path));
            }

            if (!Walls.Contains(item.Position + item.Direction))
            {
                queue.Enqueue((item.Position + item.Direction, item.Direction, item.Points + 1, item.Path.Append(item.Position + item.Direction)));
            }
        }

        return lowest;
    }

    public Day16()
    {
        Map = new Matrix(Input);
        Start = Map.Single(p => p.Value == 'S').Key;
        End = Map.Single(p => p.Value == 'E').Key;
        Walls = Map.Where(p => p.Value == '#').Select(p => p.Key).ToHashSet();
    }

    private Matrix Map { get; }
    private Vector2 Start { get; }
    private Vector2 End { get; }
    private HashSet<Vector2> Walls { get; }
}

