using System.Numerics;

public class Day08 : Day
{
    public override string Solve1() => CalculateAntinodes(recursive: false).Count.ToString();

    public override string Solve2() => CalculateAntinodes(recursive: true).Concat(Antenas.Select(x => x.Key)).Distinct().Count().ToString();

    private HashSet<Vector2> CalculateAntinodes(bool recursive)
    {
        var antinodes = new HashSet<Vector2>();

        foreach (var antena1 in Antenas)
        {
            foreach (var antena2 in Antenas.Where(x => x.Key != antena1.Key && x.Value == antena1.Value))
            {
                var delta = antena1.Key - antena2.Key;
                var queue = new Queue<Vector2>([antena1.Key]);

                while (queue.TryDequeue(out var position))
                {
                    var newNode = new Vector2(position.X + delta.X, position.Y + delta.Y);

                    if (Area.IsInside(newNode))
                    {
                        antinodes.Add(newNode);

                        if (recursive)
                        {
                            queue.Enqueue(newNode);
                        }
                    }
                }
            }
        }

        return antinodes;
    }

    public Day08()
    {
        Area = new Matrix(Input);
        Antenas = Area.Where(x => char.IsLetterOrDigit(x.Value)).ToDictionary();
    }

    private Matrix Area { get; }
    private Dictionary<Vector2, char> Antenas { get; }
}
