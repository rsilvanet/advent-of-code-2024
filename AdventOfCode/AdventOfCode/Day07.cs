public class Day07 : Day
{
    public override string Solve1() => Equations.Where(e => CanSolve(e, ['+', '*'], [])).Sum(e => e[0]).ToString();

    public override string Solve2() => Equations.Where(e => CanSolve(e, ['+', '*', '|'], [])).Sum(e => e[0]).ToString();

    private bool CanSolve(long[] equation, char[] allowedOperators, IEnumerable<char> operators)
    {
        if (equation.Length - operators.Count() == 2)
        {
            var expected = equation[0];
            var calculated = equation[1];
            var operatorArray = operators.ToArray();

            for (var i = 2; i < equation.Length; i++)
            {
                calculated = operatorArray[i - 2] switch
                {
                    '+' => calculated += equation[i],
                    '*' => calculated *= equation[i],
                    '|' => Concatenate(calculated, equation[i]),
                    _ => throw new NotImplementedException()
                };
            }

            return calculated == expected;
        }

        foreach (var allowedOperator in allowedOperators)
        {
            if (CanSolve(equation, allowedOperators, operators.Append(allowedOperator)))
            {
                return true;
            }
        }

        return false;
    }

    private static long Concatenate(long value1, long value2)
    {
        int multiplier = 1;

        while (multiplier <= value2)
        {
            multiplier *= 10;
        }

        return value1 * multiplier + value2;
    }

    public Day07()
    {
        Equations = Input.Select(line => line.Split(' ').Select(number => long.Parse(number.Trim().Trim(':'))).ToArray());
    }

    private IEnumerable<long[]> Equations { get; }
}
