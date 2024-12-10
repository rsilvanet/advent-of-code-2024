using System.Numerics;

public class Day10 : Day
{
    public override string Solve1() => Navigate().GroupBy(x => (x.First(), x.Last())).Count().ToString();

    public override string Solve2() => Navigate().Count().ToString();

    private IEnumerable<Vector2[]> Navigate()
    {
        var paths = new HashSet<Vector2[]>();
        var directions = new Vector2[] { MatrixHelper.Up, MatrixHelper.Down, MatrixHelper.Left, MatrixHelper.Right };

        foreach (var positionZero in Zeros)
        {
            var queue = new Queue<(int Height, Vector2[] Path)>([(0, [positionZero])]);

            while (queue.TryDequeue(out var lastPosition))
            {
                if (lastPosition.Height == 9)
                {
                    yield return lastPosition.Path;
                    continue;
                }

                foreach (var direction in directions)
                {
                    var nextPosition = lastPosition.Path.Last() + direction;

                    if (Map.IsInside(nextPosition) && char.GetNumericValue(Map[nextPosition]) == (lastPosition.Height + 1))
                    {
                        queue.Enqueue((Height: (int)char.GetNumericValue(Map[nextPosition]), Path: lastPosition.Path.Append(nextPosition).ToArray()));
                    }
                }
            }
        }
    }

    public Day10()
    {
        Map = new Matrix(Input);
        Zeros = Map.Where(x => x.Value == '0').Select(x => x.Key).ToArray();
    }

    private Matrix Map { get; }
    private Vector2[] Zeros { get; }
}

