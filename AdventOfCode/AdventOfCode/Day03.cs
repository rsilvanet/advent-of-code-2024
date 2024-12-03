using System.Text.RegularExpressions;

public class Day03 : Day
{
    public override string Solve1() => Input
        .SelectMany(line => MulRegex.Matches(line))
        .Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value))
        .ToString();

    public override string Solve2()
    {
        var result = 0;
        var enabled = true;

        foreach (var item in Input.SelectMany(line => MulDoDontRegex.Matches(line)))
        {
            enabled = item.Groups[0].Value switch
            {
                "do()" => true,
                "don't()" => false,
                _ => enabled
            };

            if (enabled && item.Groups[0].Value.StartsWith("mul"))
            {
                result += int.Parse(item.Groups[1].Value) * int.Parse(item.Groups[2].Value);
            }
        }

        return result.ToString();
    }

    private static readonly Regex MulRegex = new("mul\\((\\d{1,3}),(\\d{1,3})\\)");

    private static readonly Regex MulDoDontRegex = new("(?:mul\\((\\d{1,3}),(\\d{1,3})\\)|do\\(\\)|don't\\(\\))");
}

