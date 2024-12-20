using System.Numerics;

public class Day20 : Day
{
    public override string Solve1()
    {
        var steps = new List<int>();
        var queue = new Queue<(Vector2 Position, int Steps, bool HasCheated, HashSet<Vector2> Visited)>([(Start, 0, false, [])]);
        var cache = new Dictionary<(Vector2, int, bool), int>();

        while (queue.TryDequeue(out var item))
        {
            item.Visited.Add(item.Position);

            if (item.Position == End)
            {
                steps.Add(item.Steps);
                continue;
            }

            if (item.HasCheated && Map[item.Position] == '.')
            {
                steps.Add(item.Steps + CalculateRemainingTrack(item.Position));
                continue;
            }

            foreach (var nextStep in MatrixHelper.FourDirections.Select(d => item.Position + d).Where(Map.IsInside))
            {
                if (item.Visited.Contains(nextStep))
                {
                    continue;
                }

                if (!Walls.Contains(nextStep))
                {
                    queue.Enqueue((nextStep, item.Steps + 1, item.HasCheated, item.Visited.ToHashSet()));
                }

                if (!item.HasCheated)
                {
                    queue.Enqueue((nextStep, item.Steps + 1, true, item.Visited.ToHashSet()));
                }
            }
        }

        return steps.Select(x => TrackSteps - x).Count(x => x >= 100).ToString();
    }

    public override string Solve2() => 0.ToString();

    private List<Vector2> RecordTrack()
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

        return visited.ToList();
    }

    private int CalculateRemainingTrack(Vector2 position) => TrackSteps - Track.IndexOf(position);

    public Day20()
    {
        Map = new Matrix(Input);
        Walls = Map.Where(x => x.Value == '#').Select(x => x.Key).ToHashSet();
        Start = Map.Where(x => x.Value == 'S').Single().Key;
        End = Map.Where(x => x.Value == 'E').Single().Key;
        Track = RecordTrack();
        TrackSteps = Track.Count - 1;
    }

    private Matrix Map { get; }
    private HashSet<Vector2> Walls { get; }
    private Vector2 Start { get; }
    private Vector2 End { get; }
    private List<Vector2> Track { get; }
    private int TrackSteps { get; }
}

