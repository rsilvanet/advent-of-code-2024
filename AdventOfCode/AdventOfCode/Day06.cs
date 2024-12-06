using System.Numerics;

public class Day06 : Day
{
    public override string Solve1() => TryWalk(extraObstacle: Vector2.NaN, out var track) ? track.Count.ToString() : throw new Exception();

    public override string Solve2()
    {
        TryWalk(extraObstacle: Vector2.NaN, out var track);

        var counter = 0;

        foreach (var position in track.Where(p => Map[p.Key] != '^'))
        {
            counter += TryWalk(extraObstacle: position.Key, out var _) ? 0 : 1;
        }

        return counter.ToString();
    }

    private bool TryWalk(Vector2 extraObstacle, out Dictionary<Vector2, HashSet<Vector2>> visited)
    {
        visited = new Dictionary<Vector2, HashSet<Vector2>>();

        var guard = (Position: Start, Direction: MatrixHelper.Up);

        while (Map.IsInside(guard.Position))
        {
            if (visited.TryGetValue(guard.Position, out var directions) && directions.Contains(guard.Direction))
            {
                return false;
            }

            visited.TryAdd(guard.Position, []);
            visited[guard.Position].Add(guard.Direction);

            var nextPosition = guard.Position + guard.Direction;
            
            if (Obstacles.Contains(nextPosition) || nextPosition == extraObstacle)
            {
                guard.Direction = guard.Direction.TurnRight();
            }
            else
            {
                guard.Position = guard.Position + guard.Direction;
            }
        }

        return true;
    }

    public Day06()
    {
        Map = new Matrix(Input);
        Start = Map.Single(x => x.Value == '^').Key;
        Obstacles = Map.Where(x => x.Value == '#').Select(x => x.Key).ToHashSet();
    }

    private Matrix Map { get; }
    private Vector2 Start { get; }
    private HashSet<Vector2> Obstacles { get; }
}