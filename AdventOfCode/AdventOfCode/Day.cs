public abstract class Day
{
    public IEnumerable<string> Input => File.ReadLines($"./Inputs/{GetType().Name}.txt");
    public IEnumerable<string> Sample => File.ReadLines($"./Samples/{GetType().Name}.txt");
    public abstract long Solve1();
    public abstract long Solve2();
}

