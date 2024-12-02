public class Day02 : Day
{
    public override string Solve1() => Reports.Count(IsSafe).ToString();

    public override string Solve2() => Reports.Count(IsSafeWithTolerance).ToString();

    private IEnumerable<IEnumerable<int>> Reports => Input.Select(report => report.Split(' ').Select(int.Parse));

    private static bool IsSafe(IEnumerable<int> levels) => IsSafelyIncreasing(levels) || IsSafelyIncreasing(levels.Reverse());

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

    private static bool IsSafelyIncreasing(IEnumerable<int> levels)
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
}

