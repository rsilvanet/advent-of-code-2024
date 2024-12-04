public abstract class Day
{
    public IEnumerable<string> Input { get; }
    public abstract string Solve1();
    public abstract string Solve2();

    protected Day()
    {
        Input = File.ReadLines($"./Inputs/{GetType().Name}.txt");
    }
}
