public abstract class Day
{
    public IEnumerable<string> Input => File.ReadLines($"./Inputs/{GetType().Name}.txt");
    public IEnumerable<string> Sample => File.ReadLines($"./Samples/{GetType().Name}.txt");
    public abstract string Solve1();
    public abstract string Solve2();
}

