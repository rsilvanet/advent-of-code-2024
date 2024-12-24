public class Day24 : Day
{
    public override string Solve1()
    {
        var (wires, gates) = ReadInput();

        while (wires.Any(x => x.Key.StartsWith("z") && x.Value is null))
        {
            foreach (var gate in gates.Where(g => wires[g.Input1].HasValue && wires[g.Input2].HasValue && !wires[g.Output].HasValue))
            {
                wires[gate.Output] = gate.Operation switch
                {
                    "AND" => wires[gate.Input1] == 1 && wires[gate.Input2] == 1 ? 1 : 0,
                    "OR" => wires[gate.Input1] == 1 || wires[gate.Input2] == 1 ? 1 : 0,
                    "XOR" => wires[gate.Input1] != wires[gate.Input2] ? 1 : 0,
                    _ => throw new NotImplementedException()
                };
            }
        }

        return Convert.ToInt64(string.Join("", wires.Where(x => x.Key.StartsWith("z")).OrderByDescending(x => x.Key).Select(x => x.Value)), 2).ToString();
    }

    public override string Solve2() => 0.ToString();

    private (Dictionary<string, int?> Wires, List<Gate> Gates) ReadInput()
    {
        var wires = new Dictionary<string, int?>();
        var gates = new List<Gate>();

        foreach (var line in Input)
        {
            if (line.Contains(":"))
            {
                var split = line.Split(":");
                wires.Add(split[0], int.Parse(split[1]));
            }
            else if (line.Contains("->"))
            {
                var split = line.Split(" ");
                var gate = new Gate(split[0], split[1], split[2], split[4]);
                gates.Add(gate);
                wires.TryAdd(gate.Input1, null);
                wires.TryAdd(gate.Input2, null);
                wires.TryAdd(gate.Output, null);
            }
        }

        return (wires, gates);
    }

    private record Gate(string Input1, string Operation, string Input2, string Output);
}

