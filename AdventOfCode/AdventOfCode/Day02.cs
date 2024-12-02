public class Day02 : Day
{
    public override string Solve1()
    {
        var count = 0;

        foreach (var report in Input)
        {
            var levels = report.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();

            if (IsSafe(levels) || IsSafe(levels.Reverse().ToArray()))
            {
                count++;
            }
        }

        return count.ToString();
    }

    public override string Solve2()
    {
        var count = 0;

        foreach (var report in Input)
        {
            var levels = report.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();

            if (IsSafeWithTolerance(levels) || IsSafeWithTolerance(levels.Reverse().ToArray()))
            {
                count++;
            }
        }

        return count.ToString();
    }

    public bool IsSafe(int[] levels)
    {
        for (var i = 0; i < levels.Length - 1; i++)
        {
            if (levels[i + 1] - levels[i] is < 1 or > 3)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsSafeWithTolerance(int[] levels)
    {
        for (var skip = 0; skip < levels.Length; skip++)
        {
            var newLevels = levels.ToList();
            newLevels.RemoveAt(skip);

            if (IsSafe(newLevels.ToArray()))
            {
                return true;
            }
        }

        return false;
    }
}

