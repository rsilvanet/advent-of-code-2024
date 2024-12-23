public class Day23 : Day
{
    public override string Solve1() => Index.Keys
        .SelectMany(computer => FindNetworks(computer, 3, Memo))
        .DistinctBy(ToString)
        .Where(x => x.Any(computer => computer.StartsWith("t")))
        .Count()
        .ToString();

    public override string Solve2() => Index.Keys
        .SelectMany(computer => FindNetworks(computer, null, Memo))
        .Select(ToString)
        .Distinct()
        .MaxBy(x => x.Count())!;

    private List<string[]> FindNetworks(string computer, int? size, HashSet<string> memo)
    {
        var connections = new List<string[]>();
        FindNetworksRecursive(computer, size, [computer], connections, memo);
        return connections;
    }

    private void FindNetworksRecursive(string computer, int? size, HashSet<string> visited, List<string[]> networks, HashSet<string> memo)
    {
        if (visited.Count > size)
        {
            return;
        }

        var memoKey = $"{ToString(visited)}|{size}";

        if (memo.Contains(memoKey))
        {
            return;
        }

        if (!size.HasValue || visited.Count == size)
        {
            networks.Add(visited.ToArray());
        }

        foreach (var otherComputer in Index[computer])
        {
            if (visited.Contains(otherComputer) || visited.Any(x => x != computer && !Index[x].Contains(otherComputer)) || !Index.ContainsKey(otherComputer))
            {
                continue;
            }

            visited.Add(otherComputer);
            FindNetworksRecursive(otherComputer, size, visited, networks, memo);
            visited.Remove(otherComputer);
        }

        memo.Add(memoKey);
    }

    private string ToString(IEnumerable<string> computers) => string.Join(',', computers.Order());

    public Day23()
    {
        Index = new Dictionary<string, HashSet<string>>();

        foreach (var line in Input.Select(x => x.Split('-')).Select(x => (First: x.First(), Second: x.Last())))
        {
            if (!Index.TryAdd(line.First, [line.Second]))
            {
                Index[line.First].Add(line.Second);
            }

            if (!Index.TryAdd(line.Second, [line.First]))
            {
                Index[line.Second].Add(line.First);
            }
        }
    }

    private Dictionary<string, HashSet<string>> Index { get; }
    private HashSet<string> Memo { get; } = new();
}
