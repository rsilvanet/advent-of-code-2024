using System.Numerics;

public class Day16 : Day
{
    public override string Solve1() => Iterate(Start, MatrixHelper.Right, 0, int.MaxValue, []).ToString();

    public override string Solve2() => string.Empty;

    private int Iterate(Vector2 position, Vector2 direction, int points, int lowest, Dictionary<(Vector2, Vector2), int> bestAtPosition)
    {
        if ((bestAtPosition.TryGetValue((position, direction), out var best) && points >= best) || points >= lowest)
        {
            return lowest;
        }
        else
        {
            bestAtPosition[(position, direction)] = points;
        }

        if (position == End)
        {
            return Math.Min(points, lowest);
        }

        if (!Walls.Contains(position + direction.TurnRight()))
        {
            lowest = Iterate(position, direction.TurnRight(), points + 1000, lowest, bestAtPosition);
        }

        if (!Walls.Contains(position + direction.TurnLeft()))
        {
            lowest = Iterate(position, direction.TurnLeft(), points + 1000, lowest, bestAtPosition);
        }

        if (!Walls.Contains(position + direction))
        {
            lowest = Iterate(position + direction, direction, points + 1, lowest, bestAtPosition);
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

