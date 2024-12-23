public class Day23 : Day
{
    public override string Solve1()
    {
        var interConnections = Enumerable.Empty<string[]>();

        foreach (var computer in Index.Keys)
        {
            interConnections = interConnections.Concat(FindInterConnections(computer, 3));
        }

        return interConnections.DistinctBy(ToString).Count(x => x.Any(computer => computer.StartsWith("t"))).ToString();
    }

    public override string Solve2()
    {
        var interConnections = Enumerable.Empty<string[]>();

        foreach (var computer in Index.Keys)
        {
            interConnections = interConnections.Concat(FindInterConnections(computer));
        }

        return string.Join(",", interConnections.Select(ToString).Distinct().Order().MaxBy(x => x.Count())!);
    }

    private List<string[]> FindInterConnections(string computer, int size)
    {
        var connections = new List<string[]>();
        FindInterConnectionsRecursive(computer, size, [computer], connections);
        return connections;
    }

    private void FindInterConnectionsRecursive(string computer, int size, HashSet<string> visited, List<string[]> connections)
    {
        if (visited.Count == size && Index[computer].Contains(visited.First()))
        {
            connections.Add(visited.ToArray());
            return;
        }

        if (visited.Count >= size)
        {
            return;
        }

        foreach (var otherComputer in Index[computer])
        {
            if (visited.Contains(otherComputer))
            {
                continue;
            }

            visited.Add(otherComputer);
            FindInterConnectionsRecursive(otherComputer, size, visited, connections);
            visited.Remove(otherComputer);
        }
    }

    private List<string[]> FindInterConnections(string computer)
    {
        var connections = new List<string[]>();
        var reducedIndex = Index.Where(x => x.Key == computer || x.Value.Contains(computer)).ToDictionary();
        FindInterConnectionsRecursive(computer, reducedIndex, [computer], connections, []);
        return connections;
    }

    private void FindInterConnectionsRecursive(string computer, Dictionary<string, HashSet<string>> reducedIndex, HashSet<string> visited, List<string[]> connections, HashSet<string> memo)
    {
        var memoKey = ToString(visited);

        if (memo.Contains(memoKey))
        {
            return;
        }

        connections.Add(visited.ToArray());

        foreach (var otherComputer in reducedIndex[computer])
        {
            if (visited.Contains(otherComputer) || visited.Any(x => x != computer && !reducedIndex[x].Contains(otherComputer)) || !reducedIndex.ContainsKey(otherComputer))
            {
                continue;
            }

            visited.Add(otherComputer);
            FindInterConnectionsRecursive(otherComputer, reducedIndex, visited, connections, memo);
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
}
