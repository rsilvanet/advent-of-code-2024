using System.Numerics;

public class Day15 : Day
{
    public override string Solve1()
    {
        var robot = Map.Where(x => x.Value == '@').Select(x => new Movable(x.Key)).Single();
        var boxes = Map.Where(x => x.Value == 'O').Select(x => new Movable(x.Key)).ToList();

        foreach (var move in Moves)
        {
            var direction = move switch
            {
                '^' => MatrixHelper.Up,
                'v' => MatrixHelper.Down,
                '>' => MatrixHelper.Right,
                '<' => MatrixHelper.Left,
                _ => throw new NotImplementedException()
            };

            var queue = new Queue<Movable>([robot]);
            var nextPosition = robot.Positions[0] + direction;

            while (boxes.Any(b => b.Positions[0] == nextPosition))
            {
                queue.Enqueue(boxes.First(b => b.Positions[0] == nextPosition));
                nextPosition += direction;
            }

            if (Walls.Contains(nextPosition))
            {
                queue.Clear();
            }

            while (queue.TryDequeue(out var movable))
            {
                movable.Move(direction);
            }
        }

        return boxes.Sum(b => b.Positions[0].Y * 100 + b.Positions[0].X).ToString();
    }

    public override string Solve2()
    {
        var robot = ExpandedMap.Where(x => x.Value == '@').Select(x => new Movable(x.Key)).Single();
        var boxes = ExpandedMap.Where(x => x.Value == '[').Select(x => new Movable(x.Key, x.Key + MatrixHelper.Right)).ToList();

        foreach (var move in Moves)
        {
            var direction = move switch
            {
                '^' => MatrixHelper.Up,
                'v' => MatrixHelper.Down,
                '>' => MatrixHelper.Right,
                '<' => MatrixHelper.Left,
                _ => throw new NotImplementedException()
            };

            var queue = new Queue<Movable>([robot]);
            var nextPositions = robot.Positions.Select(p => p + direction);
            
            while (boxes.Any(b => !queue.Contains(b) && b.Positions.Intersect(nextPositions).Any()))
            {
                var touchingBoxes = boxes.Where(b => !queue.Contains(b) && b.Positions.Intersect(nextPositions).Any()).ToList();

                foreach (var box in touchingBoxes)
                {
                    queue.Enqueue(box);
                }

                nextPositions = touchingBoxes.SelectMany(p => p.Positions).Select(p => p + direction);

                if (nextPositions.Any(ExpandedWalls.Contains))
                {
                    break;
                }
            }

            if (nextPositions.Any(ExpandedWalls.Contains))
            {
                continue;
            }

            while (queue.TryDequeue(out var movable))
            {
                movable.Move(direction);
            }
        }

        return boxes.Sum(b => b.Positions[0].Y * 100 + b.Positions[0].X).ToString();
    }

    public Day15()
    {
        Map = new Matrix(Input.Where(line => line.StartsWith('#') || line.StartsWith('.')));
        Walls = Map.Where(x => x.Value == '#').Select(x => x.Key).ToHashSet();
        Moves = Input.Where(line => !line.StartsWith('#') && !line.StartsWith('.') && !string.IsNullOrWhiteSpace(line)).SelectMany(x => x);
        ExpandedMap = new Matrix(Input.Where(line => line.StartsWith('#')).Select(Expand));
        ExpandedWalls = ExpandedMap.Where(x => x.Value == '#').Select(x => x.Key).ToHashSet();
    }

    private string Expand(string line) => line.Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.");

    private Matrix Map { get; }
    private HashSet<Vector2> Walls { get; }
    private Matrix ExpandedMap { get; }
    private HashSet<Vector2> ExpandedWalls { get; }
    private IEnumerable<char> Moves { get; }

    private class Movable(params Vector2[] positions)
    {
        public Vector2[] Positions { get; } = positions;

        public void Move(Vector2 direction)
        {
            for (int i = 0; i < Positions.Length; i++)
            {
                Positions[i] += direction;
            }
        }

        public override string ToString() => string.Join('-', Positions.Select(x => $"{x.X},{x.Y}"));
    }
}