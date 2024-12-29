public class Day19 : Day
{
    public override string Solve1() => Patterns.Count(p => CountPatternMatches(p, Towels.Where(p.Contains).ToArray()) > 1).ToString();

    public override string Solve2() => Patterns.Sum(p => CountPatternMatches(p, Towels.Where(p.Contains).ToArray())).ToString();

    private long CountPatternMatches(string pattern, string[] parts) => CountPatternMatchesRecursive(pattern, parts, 0, []);

    private long CountPatternMatchesRecursive(string patternRemainder, string[] parts, long count, Dictionary<string, long> cache)
    {
        if (cache.TryGetValue((patternRemainder), out var cached))
        {
            return cached + count;
        }

        var previousCount = count;

        foreach (var part in parts.Where(patternRemainder.StartsWith))
        {
            if (part == patternRemainder)
            {
                count++;
            }
            else
            {
                count = CountPatternMatchesRecursive(patternRemainder[part.Length..], parts, count, cache);
            }
        }

        cache[patternRemainder] = count - previousCount;

        return count;
    }

    public Day19()
    {
        Towels = Input.ElementAt(0).Split(',').Select(x => x.Trim()).ToArray();
        Patterns = Input.Skip(2).ToArray();
    }

    private string[] Towels { get; }
    private string[] Patterns { get; }
}
