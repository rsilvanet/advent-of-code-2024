﻿using System.Numerics;

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
                this[new Vector2(column, row)] = array[row][column];
            }
        }

        MaxRow = (int)Keys.Max(k => k.Y);
        MaxColumn = (int)Keys.Max(k => k.X);
    }

    public Matrix(int maxColumn, int maxRow)
    {
        for (var row = 0; row <= maxRow; row++)
        {
            for (var column = 0; column <= maxColumn; column++)
            {
                this[new Vector2(column, row)] = '.';
            }
        }

        MaxRow = maxRow;
        MaxColumn = maxColumn;
    }

    public int MaxRow { get; }
    public int MaxColumn { get; }

    public bool HasValue(Vector2 position, char value) => TryGetValue(position, out var valueInPosition) && valueInPosition == value;

    public bool IsInside(Vector2 position) => position.X >= 0 && position.X <= MaxColumn && position.Y >= 0 && position.Y <= MaxRow;

    public void Print()
    {
        for (int row = 0; row <= MaxRow; row++)
        {
            for (int column = 0; column <= MaxColumn; column++)
            {
                Console.Write(this[new Vector2(column, row)]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
