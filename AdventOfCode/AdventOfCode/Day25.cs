using System.Numerics;

public class Day25 : Day
{
    public override string Solve1() => Locks.Sum(@lock => Keys.Count(key => Fit(@lock, key))).ToString();

    public override string Solve2() => 0.ToString();

    private bool Fit(Matrix @lock, Matrix key)
    {
        for (var y = 0; y <= key.MaxRow; y++)
        {
            for (var x = 0; x <= key.MaxColumn; x++)
            {
                if (@lock[new Vector2(x, y)] == '#' && @key[new Vector2(x, y)] == '#')
                {
                    return false;
                }
            }
        }

        return true;
    }

    public Day25()
    {
        var block = new List<string>();

        for (int i = 0; i < Input.Count(); i++)
        {
            var line = Input.ElementAt(i);

            if (string.IsNullOrWhiteSpace(line) || i == Input.Count() - 1)
            {
                var matrix = new Matrix(block);

                if (matrix.Where(x => x.Key.Y == 0).All(x => x.Value == '#'))
                {
                    Locks.Add(matrix);
                }
                else
                {
                    Keys.Add(matrix);
                }

                block = new List<string>();
                continue;
            }

            block.Add(line);
        }
    }

    private List<Matrix> Keys { get; } = new();
    private List<Matrix> Locks { get; } = new();
}