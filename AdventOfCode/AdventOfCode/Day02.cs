public class Day02 : Day
{
    public override string Solve1() => Input
        .Select(report => report.Split(' ').Select(int.Parse))
        .Count(levels => IsSafe(levels) || IsSafe(levels.Reverse()))
        .ToString();

    public override string Solve2() => Input
        .Select(report => report.Split(' ').Select(int.Parse))
        .Count(levels => IsSafeWithTolerance(levels) || IsSafeWithTolerance(levels.Reverse()))
        .ToString();

    private static bool IsSafe(IEnumerable<int> levels)
    {
        for (var index = 0; index < levels.Count() - 1; index++)
        {
            if (levels.ElementAt(index + 1) - levels.ElementAt(index) is < 1 or > 3)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsSafeWithTolerance(IEnumerable<int> levels)
    {
        for (var indexToSkip = 0; indexToSkip < levels.Count(); indexToSkip++)
        {
            if (IsSafe(levels.Where((_, index) => index != indexToSkip)))
            {
                return true;
            }
        }

        return false;
    }
}

