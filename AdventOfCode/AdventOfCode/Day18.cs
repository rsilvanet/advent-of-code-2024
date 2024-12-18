using System.Numerics;

public class Day18 : Day
{
    public override string Solve1() => Navigate(1024).ToString();

    public override string Solve2()
    {
        var lastPossibleAmount = 1024;
        var firstImpossibleAmount = FallingBytes.Count();

        while (firstImpossibleAmount - lastPossibleAmount > 1)
        {
            var halfAmount = lastPossibleAmount + ((firstImpossibleAmount - lastPossibleAmount) / 2);

            if (Navigate(halfAmount) == int.MaxValue)
            {
                firstImpossibleAmount = halfAmount;
            }
            else
            {
                lastPossibleAmount = halfAmount;
            }
        }

        return FallingBytes.ElementAt(lastPossibleAmount) is Vector2 lastPossibleByte ? $"{lastPossibleByte.X},{lastPossibleByte.Y}" : string.Empty;
    }

    private int Navigate(int amountOfBytes)
    {
        var fallingBytes = FallingBytes.Take(amountOfBytes).ToHashSet();
        var visited = new HashSet<Vector2>([new Vector2(0, 0)]);
        var queue = new Queue<(Vector2 Position, int Steps)>([(Position: new Vector2(0, 0), Steps: 0)]);
        var fewestSteps = int.MaxValue;

        while (queue.TryDequeue(out var item))
        {
            if (item.Position.X == Memory.MaxColumn && item.Position.Y == Memory.MaxColumn)
            {
                fewestSteps = Math.Min(fewestSteps, item.Steps);
                continue;
            }

            foreach (var nextPosition in MatrixHelper.FourDirections.Select(d => item.Position + d))
            {
                if (!Memory.IsInside(nextPosition) || visited.Contains(nextPosition) || fallingBytes.Contains(nextPosition))
                {
                    continue;
                }

                queue.Enqueue((nextPosition, item.Steps + 1));
                visited.Add(nextPosition);
            }
        }

        return fewestSteps;
    }

    public Day18()
    {
        Memory = new Matrix(70, 70);
        FallingBytes = Input.Select(line => line.Split(',').Select(int.Parse)).Select(split => new Vector2(split.First(), split.Last()));
    }

    private Matrix Memory { get; }
    private IEnumerable<Vector2> FallingBytes { get; }
}

