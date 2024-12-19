public class Day19 : Day
{
    public override string Solve1()
    {
        var count = 0;
        var towels = ReduceTowels(Towels);

        foreach (var pattern in Patterns)
        {
            count += CanFindPattern(pattern, towels) ? 1 : 0;
        }

        return count.ToString();
    }

    public override string Solve2() => 0.ToString();

    public string[] ReduceTowels(string[] towels)
    {
        var previousCount = towels.Count();
        var reducedTowels = towels.Where(t => !CanFindPattern(t, Towels.Except([t]))).ToArray();

        while (previousCount != reducedTowels.Count())
        {
            previousCount = reducedTowels.Count();
            reducedTowels = reducedTowels.Where(t => !CanFindPattern(t, reducedTowels.Except([t]))).ToArray();
        }

        return reducedTowels;
    }

    private bool CanFindPattern(string pattern, IEnumerable<string> parts)
    {
        var queue = new Queue<string>([pattern]);

        while (queue.TryDequeue(out var partialPattern))
        {
            foreach (var towel in parts.Where(partialPattern.StartsWith))
            {
                if (towel == partialPattern)
                {
                    return true;
                }

                queue.Enqueue(partialPattern[towel.Length..]);
            }
        }

        return false;
    }

    public Day19()
    {
        Towels = Input.ElementAt(0).Split(',').Select(x => x.Trim()).ToArray();
        Patterns = Input.Skip(2).ToArray();
    }

    private string[] Towels { get; }
    private string[] Patterns { get; }
}

