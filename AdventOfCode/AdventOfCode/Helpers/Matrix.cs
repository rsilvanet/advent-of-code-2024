using System.Numerics;

public class Matrix : Dictionary<Vector2, char>
{
    public Matrix(IEnumerable<string> input)
    {
        var array = input.ToArray();
        var rowCount = array.Length;

        for (var row = 0; row < rowCount; row++)
        {
            for (var column = 0; column < array[row].Length; column++)
            {
                this[new Vector2(row, column)] = array[row][column];
            }
        }

        MaxRow = (int)Keys.Max(k => k.Y);
        MaxColumn = (int)Keys.Max(k => k.X);
    }

    public int MaxRow { get; }
    public int MaxColumn { get; }
    public bool HasValue(Vector2 position, char value) => TryGetValue(position, out var valueInPosition) && valueInPosition == value;
}
