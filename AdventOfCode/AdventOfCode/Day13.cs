using System.Text.RegularExpressions;

public class Day13 : Day
{
    public override string Solve1() => Machines.Sum(Play).ToString();

    public override string Solve2() => HarderMachines.Sum(Play).ToString();

    private long Play(Machine machine)
    {
        long ax = machine.ButtonA.X, bx = machine.ButtonB.X, px = machine.Prize.X;
        long ay = machine.ButtonA.Y, by = machine.ButtonB.Y, py = machine.Prize.Y;
        long countB = (py * ax - ay * px) / (ax * by - ay * bx);
        long countA = (px - bx * countB) / ax;

        if (machine.Prize.X == machine.ButtonA.X * countA + machine.ButtonB.X * countB && machine.Prize.Y == machine.ButtonA.Y * countA + machine.ButtonB.Y * countB)
        {
            return countA * 3 + countB;
        }

        return 0;
    }

    public Day13()
    {
        var buttonRegex = new Regex("([A-Z])\\+(\\d+)");
        var prizeRegex = new Regex("([A-Z])\\=(\\d+)");

        for (int i = 0; i < Input.Count(); i = i + 4)
        {
            var buttonA = buttonRegex.Matches(Input.ElementAt(i));
            var buttonB = buttonRegex.Matches(Input.ElementAt(i + 1));
            var prize = prizeRegex.Matches(Input.ElementAt(i + 2));

            Machines.Add(new Machine(
                ButtonA: new LongVector2(int.Parse(buttonA[0].Groups[2].Value), int.Parse(buttonA[1].Groups[2].Value)),
                ButtonB: new LongVector2(int.Parse(buttonB[0].Groups[2].Value), int.Parse(buttonB[1].Groups[2].Value)),
                Prize: new LongVector2(int.Parse(prize[0].Groups[2].Value), int.Parse(prize[1].Groups[2].Value))
            ));

            HarderMachines.Add(new Machine(
                ButtonA: new LongVector2(int.Parse(buttonA[0].Groups[2].Value), int.Parse(buttonA[1].Groups[2].Value)),
                ButtonB: new LongVector2(int.Parse(buttonB[0].Groups[2].Value), int.Parse(buttonB[1].Groups[2].Value)),
                Prize: new LongVector2(int.Parse(prize[0].Groups[2].Value) + 10000000000000, int.Parse(prize[1].Groups[2].Value) + 10000000000000)
            ));
        }
    }

    private List<Machine> Machines { get; } = new();
    private List<Machine> HarderMachines { get; } = new();
    private record Machine(LongVector2 ButtonA, LongVector2 ButtonB, LongVector2 Prize);
    private record LongVector2(long X, long Y);
}

