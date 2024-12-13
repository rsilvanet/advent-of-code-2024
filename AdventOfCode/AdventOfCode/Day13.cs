using System.Numerics;
using System.Text.RegularExpressions;

public class Day13 : Day
{
    public override string Solve1() => Machines.Select(m => Play(m, new Vector2(0, 0), Memo)).Where(x => x < int.MaxValue).Sum().ToString();

    public override string Solve2() => 0.ToString();

    private int Play(Machine machine, Vector2 position, Dictionary<(Vector2, int), int> memo, int cost = 0, int cheapest = int.MaxValue)
    {
        if (memo.TryGetValue((position, cost), out var cached))
        {
            return cached;
        }
        
        if (position == machine.Prize)
        {
            return Math.Min(cost, cheapest);
        }
        
        if (position.X > machine.Prize.X || position.Y > machine.Prize.Y)
        {
            return Math.Min(int.MaxValue, cheapest);
        }

        cheapest = Play(machine, position + machine.ButtonA, memo, cost + 3, cheapest);
        cheapest = Play(machine, position + machine.ButtonB, memo, cost + 1, cheapest);

        memo[(position, cost)] = cheapest;

        return cheapest;
    }

    public Day13()
    {
        var buttonRegex = new Regex("([A-Z])\\+(\\d+)");
        var priceRegex = new Regex("([A-Z])\\=(\\d+)");

        for (int i = 0; i < Input.Count(); i = i + 4)
        {
            var buttonA = buttonRegex.Matches(Input.ElementAt(i));
            var buttonB = buttonRegex.Matches(Input.ElementAt(i + 1));
            var prize = priceRegex.Matches(Input.ElementAt(i + 2));

            Machines.Add(new Machine(
                ButtonA: new Vector2(int.Parse(buttonA[0].Groups[2].Value), int.Parse(buttonA[1].Groups[2].Value)),
                ButtonB: new Vector2(int.Parse(buttonB[0].Groups[2].Value), int.Parse(buttonB[1].Groups[2].Value)),
                Prize: new Vector2(int.Parse(prize[0].Groups[2].Value), int.Parse(prize[1].Groups[2].Value))
            ));
        }
    }

    private List<Machine> Machines { get; } = new();
    private Dictionary<(Vector2, int), int> Memo => new();

    private record Machine(Vector2 ButtonA, Vector2 ButtonB, Vector2 Prize);
}

