using System.Numerics;

public class Day20 : Day
{
    public override string Solve1() => Race().Select(x => TrackSteps - x).Count(x => x >= 100).ToString();

    public override string Solve2() => 0.ToString();

    private IEnumerable<int> Race()
    {
        var queue = new Queue<(Vector2 Position, int Steps, bool HasCheated, Vector2 PreviousPosition)>([(Start, 0, false, Start)]);

        while (queue.TryDequeue(out var item))
        {
            if (item.Position == End)
            {
                yield return item.Steps;
            }
            else if (item.HasCheated && Map[item.Position] == '.')
            {
                yield return item.Steps + CalculateRemainingTrack(item.Position);
            }
            else
            {
                var nextPositions = MatrixHelper.FourDirections
                    .Select(d => item.Position + d)
                    .Where(p => p != item.PreviousPosition)
                    .Where(Map.IsInside);

                foreach (var nextStep in nextPositions)
                {
                    if (!Walls.Contains(nextStep))
                    {
                        queue.Enqueue((nextStep, item.Steps + 1, item.HasCheated, item.Position));
                    }
                    else if (!item.HasCheated)
                    {
                        queue.Enqueue((nextStep, item.Steps + 1, true, item.Position));
                    }
                }
            }
        }
    }

    private IDictionary<Vector2, int> ReadFullTrack()
    {
        var queue = new Queue<Vector2>([Start]);
        var visited = new HashSet<Vector2>([Start]);

        while (queue.TryDequeue(out var position))
        {
            visited.Add(position);

            if (position == End)
            {
                break;
            }

            foreach (var nextStep in MatrixHelper.FourDirections.Select(d => position + d))
            {
                if (visited.Contains(nextStep) || Walls.Contains(nextStep))
                {
                    continue;
                }

                queue.Enqueue(nextStep);
            }
        }

        return visited.Select((x, i) => (Position: x, Index: i)).ToDictionary(x => x.Position, x => x.Index);
    }

    private int CalculateRemainingTrack(Vector2 position) => TrackSteps - Track[position];

    public Day20()
    {
        Map = new Matrix(Input);
        Walls = Map.Where(x => x.Value == '#').Select(x => x.Key).ToHashSet();
        Start = Map.Where(x => x.Value == 'S').Single().Key;
        End = Map.Where(x => x.Value == 'E').Single().Key;
        Track = ReadFullTrack();
        TrackSteps = Track.Count - 1;
    }

    private Matrix Map { get; }
    private HashSet<Vector2> Walls { get; }
    private Vector2 Start { get; }
    private Vector2 End { get; }
    private IDictionary<Vector2, int> Track { get; }
    private int TrackSteps { get; }
}