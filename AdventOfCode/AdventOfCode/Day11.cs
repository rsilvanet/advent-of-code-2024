public class Day11 : Day
{
    public override string Solve1() => Stones.Select(stone => Blink(stone, 0, 25, Memo)).Sum().ToString();

    public override string Solve2() => Stones.Select(stone => Blink(stone, 0, 75, Memo)).Sum().ToString();

    private static long Blink(long stone, int blinkCount, int blinkMax, Dictionary<(long, int), long> memo)
    {
        if (memo.TryGetValue((stone, blinkCount), out var cached))
        {
            return cached;
        }

        if (blinkCount == blinkMax)
        {
            return 1;
        }

        var count = 0L;

        if (stone == 0)
        {
            count += Blink(1, blinkCount + 1, blinkMax, memo);
        }
        else if (stone.ToString() is string numberStr && numberStr.Length % 2 == 0)
        {
            var newLength = numberStr.Length / 2;
            var firstNewStone = long.Parse(numberStr.Substring(0, newLength));
            var secondNewStone = long.Parse(numberStr.Substring(newLength, newLength));

            count += Blink(firstNewStone, blinkCount + 1, blinkMax, memo);
            count += Blink(secondNewStone, blinkCount + 1, blinkMax, memo);
        }
        else
        {
            count += Blink(stone * 2024, blinkCount + 1, blinkMax, memo);
        }

        memo.Add((stone, blinkCount), count);

        return count;
    }

    public Day11()
    {
        Stones = Input.First().Split(' ').Select(long.Parse);
    }

    private IEnumerable<long> Stones { get; }
    private Dictionary<(long, int), long> Memo => new();
}

