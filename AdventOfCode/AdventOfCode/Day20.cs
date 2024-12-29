using System.Numerics;

public class Day20 : Day
{
    public override string Solve1() => Race(2).Select(x => TrackSteps - x.Steps).Count(x => x >= 100).ToString(); // 1452

    public override string Solve2() => Race(20).Select(x => TrackSteps - x.Steps).Count(x => x >= 100).ToString(); // 999556

    private IEnumerable<(int Steps, Vector2? CheatStart, Vector2? CheatEnd)> Race(int cheatLength)
    {
        var queue = new Queue<(Vector2 Position, int Steps, Vector2 PreviousPosition, Vector2? CheatStart, Vector2? CheatEnd)>([(Start, 0, Start, null, null)]);

        while (queue.TryDequeue(out var item))
        {
            if (item.Position == End)
            {
                yield return (item.Steps, item.CheatStart, item.CheatEnd);
            }
            else if (item.CheatStart.HasValue && TrackIndexes.ContainsKey(item.Position))
            {
                yield return (item.Steps + CalculateRemainingTrack(item.Position), item.CheatStart, item.CheatEnd);
            }
            else
            {
                foreach (var nextPosition in GetPositionsWithinManhattanDistance(item.Position, cheatLength))
                {
                    if (TrackIndexes.ContainsKey(nextPosition) && nextPosition != item.Position && nextPosition != item.PreviousPosition)
                    {
                        var distance = GetManhattanDistance(item.Position, nextPosition);

                        if (distance == 1)
                        {
                            queue.Enqueue((nextPosition, item.Steps + 1, item.Position, item.CheatStart, item.CheatEnd));
                        }
                        else if (!item.CheatStart.HasValue)
                        {
                            queue.Enqueue((nextPosition, item.Steps + distance, item.Position, item.Position, nextPosition));
                        }
                    }
                }
            }
        }
    }

    private IEnumerable<Vector2> GetPositionsWithinManhattanDistance(Vector2 position, int distance)
    {
        for (int x = -distance; x <= distance; x++)
        {
            int yRange = distance - Math.Abs(x);

            for (int y = -yRange; y <= yRange; y++)
            {
                yield return position + new Vector2(x, y);
            }
        }
    }

    private int GetManhattanDistance(Vector2 start, Vector2 end) => (int)(Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.Y));

    private int CalculateRemainingTrack(Vector2 position) => TrackSteps - TrackIndexes[position];

    private IDictionary<Vector2, int> ReadFullTrack()
    {
        var visited = new HashSet<Vector2>([Start]);

        while (!visited.Contains(End))
        {
            foreach (var nextStep in MatrixHelper.FourDirections.Select(d => visited.Last() + d))
            {
                if (Walls.Contains(nextStep) || visited.Contains(nextStep))
                {
                    continue;
                }

                visited.Add(nextStep);
            }
        }

        return visited.Select((x, i) => (Position: x, Index: i)).ToDictionary(x => x.Position, x => x.Index);
    }

    public Day20()
    {
        Map = new Matrix(Input);
        Walls = Map.Where(x => x.Value == '#').Select(x => x.Key).ToHashSet();
        Start = Map.Where(x => x.Value == 'S').Single().Key;
        End = Map.Where(x => x.Value == 'E').Single().Key;
        TrackIndexes = ReadFullTrack();
        TrackSteps = TrackIndexes.Count - 1;
    }

    private Matrix Map { get; }
    private HashSet<Vector2> Walls { get; }
    private Vector2 Start { get; }
    private Vector2 End { get; }
    private IDictionary<Vector2, int> TrackIndexes { get; }
    private int TrackSteps { get; }
}
