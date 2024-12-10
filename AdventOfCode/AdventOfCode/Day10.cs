using System.Numerics;

public class Day10 : Day
{
    public override string Solve1() => Navigate().GroupBy(x => (x.First(), x.Last())).Count().ToString();

    public override string Solve2() => Navigate().Count().ToString();

    private IEnumerable<Vector2[]> Navigate()
    {
        var directions = new Vector2[] { MatrixHelper.Up, MatrixHelper.Down, MatrixHelper.Left, MatrixHelper.Right };
        var queue = new Queue<(int Height, Vector2[] Path)>(Zeros.Select(position => (Height: 0, Path: new Vector2[] { position })));

        while (queue.TryDequeue(out var position))
        {
            if (position.Height == 9)
            {
                yield return position.Path;
                continue;
            }

            foreach (var direction in directions)
            {
                var nextPosition = position.Path.Last() + direction;

                if (Map.IsInside(nextPosition) && char.GetNumericValue(Map[nextPosition]) == (position.Height + 1))
                {
                    queue.Enqueue((Height: (int)char.GetNumericValue(Map[nextPosition]), Path: position.Path.Append(nextPosition).ToArray()));
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

