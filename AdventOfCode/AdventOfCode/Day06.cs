using System.Numerics;

public class Day06 : Day
{
    public override string Solve1() => TryWalk(out var visited)
        ? visited.DistinctBy(v => v.Position).Count().ToString()
        : throw new Exception();

    public override string Solve2() => TryWalk(out var visited)
        ? visited.DistinctBy(v => v.Position).Where(v => v.Position != Start).Sum(v => TryWalk(out var _, v.Position) ? 0 : 1).ToString()
        : throw new Exception();

    private bool TryWalk(out HashSet<(Vector2 Position, Vector2 Direction)> visited, Vector2? extraObstacle = null)
    {
        visited = new HashSet<(Vector2 Position, Vector2 Direction)>();

        var guard = (Position: Start, Direction: MatrixHelper.Up);

        while (Map.IsInside(guard.Position))
        {
            if (visited.Contains((guard.Position, guard.Direction)))
            {
                return false;
            }

            visited.Add((guard.Position, guard.Direction));

            var nextPosition = guard.Position + guard.Direction;

            if (nextPosition == extraObstacle || Obstacles.Contains(nextPosition))
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