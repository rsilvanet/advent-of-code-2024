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
            var nextPosition = robot.Position + direction;

            while (boxes.Any(b => b.Position == nextPosition))
            {
                queue.Enqueue(boxes.First(b => b.Position == nextPosition));
                nextPosition += direction;
            }

            if (Walls.Contains(nextPosition))
            {
                queue.Clear();
            }

            while (queue.TryDequeue(out var movable))
            {
                movable.Position += direction;
            }
        }

        return boxes.Sum(b => b.Position.Y * 100 + b.Position.X).ToString();
    }

    public override string Solve2() => 0.ToString();

    private void Print(Vector2 robot, List<Vector2> boxes)
    {
        for (int row = 0; row <= Map.MaxRow; row++)
        {
            for (int column = 0; column <= Map.MaxColumn; column++)
            {
                if (robot.X == column && robot.Y == row)
                {
                    Console.Write('@');
                }
                else if (boxes.Any(b => b.X == column && b.Y == row))
                {
                    Console.Write('O');
                }
                else if (Walls.Any(w => w.X == column && w.Y == row))
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public Day15()
    {
        Map = new Matrix(Input.Where(line => line.StartsWith('#')));
        Walls = Map.Where(x => x.Value == '#').Select(x => x.Key).ToHashSet();
        Moves = Input.Where(line => !line.StartsWith('#') && !string.IsNullOrWhiteSpace(line)).SelectMany(x => x);
    }

    private Matrix Map { get; }
    private HashSet<Vector2> Walls { get; }
    private IEnumerable<char> Moves { get; }

    private class Movable(Vector2 position)
    {
        public Vector2 Position { get; set; } = position;
    }
}