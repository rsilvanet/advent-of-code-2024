public class Day24 : Day
{
    public override string Solve1()
    {
        var (wires, gates) = ReadInput();
        Process(wires, gates);
        return ReadWireValue(wires, "z").ToString();
    }

    public override string Solve2()
    {
        var (wires, gates) = ReadInput();

        var zGates = gates.Where(g => g.Output.StartsWith('z')).OrderBy(g => g.Output).SkipLast(1);
        var nonEdgeGates = gates.Where(x => !x.Input1.StartsWith("x") && !x.Input1.StartsWith("y") && !x.Output.StartsWith("z"));
        var nonZeroGates = gates.Where(x => !x.Input1.EndsWith("00"));

        var zGatesNotFedByXor = zGates.Where(x => x.Operation != "XOR");
        var nonEdgeGatesFedByXor = nonEdgeGates.Where(x => x.Operation == "XOR");
        var xorGatesFeedingOr = nonZeroGates.Where(c => c.Operation == "XOR" && gates.Any(n => IsFeedingGate(c, n) && n.Operation == "OR"));
        var andGatesNotFeedingOr = nonZeroGates.Where(c => c.Operation == "AND" && gates.Any(n => IsFeedingGate(c, n) && n.Operation != "OR"));
        var badGates = zGatesNotFedByXor.Concat(nonEdgeGatesFedByXor).Concat(xorGatesFeedingOr).Concat(andGatesNotFeedingOr).Distinct();

        return string.Join(",", badGates.Select(g => g.Output).Order());
    }

    private void Process(Dictionary<string, int?> wires, List<Gate> gates)
    {
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
    }

    private long ReadWireValue(Dictionary<string, int?> wires, string letter)
    {
        var relevantWires = wires.Where(x => x.Key.StartsWith(letter)).OrderByDescending(x => x.Key).Select(x => x.Value);
        var binaryNumber = string.Join("", relevantWires);

        return Convert.ToInt64(binaryNumber, 2);
    }

    private bool IsFeedingGate(Gate current, Gate next) => current.Output == next.Input1 || current.Output == next.Input2;

    private (Dictionary<string, int?> Wires, List<Gate> Gates) ReadInput()
    {
        var gates = new List<Gate>();
        var wires = new Dictionary<string, int?>();

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
