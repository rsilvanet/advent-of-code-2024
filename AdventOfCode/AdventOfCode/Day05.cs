public class Day05 : Day
{
    public override string Solve1() => Pages
        .Where(page => Rules.Where(rule => IsApplicable(page, rule)).All(rule => IsValid(page, rule)))
        .Sum(page => page[page.Count / 2])
        .ToString();

    public override string Solve2()
    {
        var result = 0;

        foreach (var page in Pages.Where(page => Rules.Where(rule => IsApplicable(page, rule) && !IsValid(page, rule)).Any()))
        {
            while (Rules.FirstOrDefault(rule => IsApplicable(page, rule) && !IsValid(page, rule)) is List<int> rule)
            {
                var (firstIndex, secondIndex) = (page.IndexOf(rule[0]), page.IndexOf(rule[1]));
                (page[firstIndex], page[secondIndex]) = (page[secondIndex], page[firstIndex]);
            }

            result += page[page.Count / 2];
        }

        return result.ToString();
    }

    public Day05()
    {
        Rules = Input.Where(line => line.Contains("|")).Select(line => line.Split("|").Select(int.Parse).ToList()).ToList();
        Pages = Input.Where(line => line.Contains(",")).Select(line => line.Split(",").Select(int.Parse).ToList()).ToList();
    }

    private IEnumerable<List<int>> Rules { get; }
    private IEnumerable<List<int>> Pages { get; }

    private static bool IsApplicable(List<int> page, List<int> rule) => page.Contains(rule[0]) && page.Contains(rule[1]);
    private static bool IsValid(List<int> page, List<int> rule) => page.IndexOf(rule[1]) > page.IndexOf(rule[0]);
}

