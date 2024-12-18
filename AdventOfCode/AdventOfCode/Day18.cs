using System.Numerics;

public class Day18 : Day
{
    public override string Solve1()
    {
        var memory = new Matrix(70, 70);
        var fallingBytes = FallingBytes.Take(1024).ToHashSet();
        var visited = new HashSet<Vector2>([new Vector2(0, 0)]);
        var queue = new Queue<(Vector2 Position, int Steps)>([(Position: new Vector2(0, 0), Steps: 0)]);
        var fewestSteps = int.MaxValue;

        while (queue.TryDequeue(out var item))
        {
            if (item.Position.X == memory.MaxColumn && item.Position.Y == memory.MaxColumn)
            {
                fewestSteps = Math.Min(fewestSteps, item.Steps);
                continue;
            }

            foreach (var nextPosition in MatrixHelper.FourDirections.Select(d => item.Position + d))
            {
                if (!memory.IsInside(nextPosition) || visited.Contains(nextPosition) || fallingBytes.Contains(nextPosition))
                {
                    continue;
                }

                queue.Enqueue((nextPosition, item.Steps + 1));
                visited.Add(nextPosition);
            }
        }

        return fewestSteps.ToString();
    }

    public override string Solve2() => 0.ToString();

    public Day18()
    {
        FallingBytes = Input.Select(line => line.Split(',').Select(int.Parse)).Select(split => new Vector2(split.First(), split.Last()));
    }

    private IEnumerable<Vector2> FallingBytes { get; }
}

