using System.Numerics;

public class Day08 : Day
{
    public override string Solve1()
    {
        var antinodes = new HashSet<Vector2>();

        foreach (var antena in Antenas)
        {
            var otherAntenas = Antenas.Where(x => x.Key != antena.Key && x.Value == antena.Value);

            foreach (var otherAntena in otherAntenas)
            {
                var delta = antena.Key - otherAntena.Key;
                var newNode = new Vector2(antena.Key.X + delta.X, antena.Key.Y + delta.Y);

                if (Area.IsInside(newNode))
                {
                    antinodes.Add(newNode);
                }
            }
        }

        return antinodes.Count.ToString();
    }

    public override string Solve2()
    {
        var antinodes = new HashSet<Vector2>();

        foreach (var antena in Antenas)
        {
            var otherAntenas = Antenas.Where(x => x.Key != antena.Key && x.Value == antena.Value);

            foreach (var otherAntena in otherAntenas)
            {
                var queue = new Queue<(Vector2 Antena1, Vector2 Antena2)>();
                queue.Enqueue((antena.Key, otherAntena.Key));

                while (queue.TryDequeue(out var item))
                {
                    var delta = item.Antena1 - item.Antena2;
                    var newNode = new Vector2(item.Antena1.X + delta.X, item.Antena1.Y + delta.Y);

                    if (Area.IsInside(newNode))
                    {
                        antinodes.Add(newNode);
                    }
                    else
                    {
                        break;
                    }

                    queue.Enqueue((newNode, item.Antena1));
                }
            }
        }

        return antinodes.Concat(Antenas.Select(x => x.Key)).Distinct().Count().ToString();
    }

    public Day08()
    {
        Area = new Matrix(Input);
        Antenas = Area.Where(x => char.IsLetterOrDigit(x.Value)).ToDictionary();
    }

    private Matrix Area { get; }
    private Dictionary<Vector2, char> Antenas { get; }
}
