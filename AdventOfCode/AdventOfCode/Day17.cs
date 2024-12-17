public class Day17 : Day
{
    public override string Solve1()
    {
        var (registers, program) = Initialize();
        return string.Join(',', Run(registers, program));
    }

    public override string Solve2() => 0.ToString();

    private List<long> Run(Dictionary<char, long> registers, long[] program, bool haltWhenOutputDoesntMatchProgram = false)
    {
        var pointer = 0L;
        var output = new List<long>();

        while (pointer < program.Length)
        {
            if (program[pointer] == 0)
            {
                registers['A'] = PerformDivision(registers, program, pointer);
            }
            else if (program[pointer] == 1)
            {
                registers['B'] = registers['B'] ^ program[pointer + 1];
            }
            else if (program[pointer] == 2)
            {
                registers['B'] = GetComboOperandValue(program[pointer + 1], registers) % 8;
            }
            else if (program[pointer] == 3)
            {
                if (registers['A'] != 0)
                {
                    pointer = program[pointer + 1];
                    continue;
                }
            }
            else if (program[pointer] == 4)
            {
                registers['B'] = registers['B'] ^ registers['C'];
            }
            else if (program[pointer] == 5)
            {
                output.Add(GetComboOperandValue(program[pointer + 1], registers) % 8);

                if (haltWhenOutputDoesntMatchProgram && !Enumerable.SequenceEqual(output, program.Take(output.Count)))
                {
                    return output;
                }
            }
            else if (program[pointer] == 6)
            {
                registers['B'] = PerformDivision(registers, program, pointer);
            }
            else if (program[pointer] == 7)
            {
                registers['C'] = PerformDivision(registers, program, pointer);
            }
            else
            {
                throw new NotImplementedException();
            }

            pointer += 2;
        }

        return output;
    }

    private long GetComboOperandValue(long operand, Dictionary<char, long> registers) => operand switch
    {
        4 => registers['A'],
        5 => registers['B'],
        6 => registers['C'],
        _ when operand is >= 0 and <= 3 => operand,
        _ => throw new NotImplementedException()
    };

    private int PerformDivision(Dictionary<char, long> registers, long[] program, long pointer)
    {
        var numerator = registers['A'];
        var denominator = (long)Math.Pow(2, GetComboOperandValue(program[pointer + 1], registers));

        return denominator > 0 ? (int)Math.Min(int.MaxValue, numerator / denominator) : 0;
    }

    private (Dictionary<char, long> registers, long[] program) Initialize()
    {
        var registers = new Dictionary<char, long>()
        {
            { 'A', int.Parse(Input.ElementAt(0).Split(':').Last().Trim()) },
            { 'B', int.Parse(Input.ElementAt(1).Split(':').Last().Trim()) },
            { 'C', int.Parse(Input.ElementAt(2).Split(':').Last().Trim()) }
        };

        return (registers, Input.ElementAt(4).Split(':').Last().Trim().Split(',').Select(long.Parse).ToArray());
    }
}

