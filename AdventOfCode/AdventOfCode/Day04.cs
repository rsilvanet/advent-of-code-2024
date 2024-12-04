using System.Numerics;

public class Day04 : Day
{
    public override string Solve1() => Matrix
        .Where(pos => pos.Value == 'X')
        .Sum(pos => Requirements1.Count(dir => dir.All(step => Matrix.HasValue(pos.Key + step.Item1, step.Item2))))
        .ToString();

    public override string Solve2() => Matrix
        .Where(pos => pos.Value == 'A')
        .Count(pos => Requirements2.Count(dir => dir.All(step => Matrix.HasValue(pos.Key + step.Item1, step.Item2))) == 2)
        .ToString();

    public Day04()
    {
        Matrix = new Matrix(Input);

        Requirements1 = new List<List<(Vector2, char)>>
        {
            new List<(Vector2, char)>() { (MatrixHelper.Up, 'M'), (MatrixHelper.Up.At(2), 'A'), (MatrixHelper.Up.At(3), 'S') },
            new List<(Vector2, char)>() { (MatrixHelper.Down, 'M'), (MatrixHelper.Down.At(2), 'A'), (MatrixHelper.Down.At(3), 'S') },
            new List<(Vector2, char)>() { (MatrixHelper.Left, 'M'), (MatrixHelper.Left.At(2), 'A'), (MatrixHelper.Left.At(3), 'S') },
            new List<(Vector2, char)>() { (MatrixHelper.Right, 'M'), (MatrixHelper.Right.At(2), 'A'), (MatrixHelper.Right.At(3), 'S') },
            new List<(Vector2, char)>() { (MatrixHelper.UpperLeft, 'M'), (MatrixHelper.UpperLeft.At(2), 'A'), (MatrixHelper.UpperLeft.At(3), 'S') },
            new List<(Vector2, char)>() { (MatrixHelper.UpperRight, 'M'), (MatrixHelper.UpperRight.At(2), 'A'), (MatrixHelper.UpperRight.At(3), 'S') },
            new List<(Vector2, char)>() { (MatrixHelper.BottomLeft, 'M'), (MatrixHelper.BottomLeft.At(2), 'A'), (MatrixHelper.BottomLeft.At(3), 'S') },
            new List<(Vector2, char)>() { (MatrixHelper.BottomRight, 'M'), (MatrixHelper.BottomRight.At(2), 'A'), (MatrixHelper.BottomRight.At(3), 'S') },
        };

        Requirements2 = new List<List<(Vector2, char)>>
        {
            new List<(Vector2, char)>() { (MatrixHelper.UpperLeft, 'M'), (MatrixHelper.BottomRight, 'S') },
            new List<(Vector2, char)>() { (MatrixHelper.UpperLeft, 'S'), (MatrixHelper.BottomRight, 'M') },
            new List<(Vector2, char)>() { (MatrixHelper.BottomLeft, 'M'), (MatrixHelper.UpperRight, 'S') },
            new List<(Vector2, char)>() { (MatrixHelper.BottomLeft, 'S'), (MatrixHelper.UpperRight, 'M') },
        };
    }

    public Matrix Matrix { get; }
    public List<List<(Vector2, char)>> Requirements1 { get; }
    public List<List<(Vector2, char)>> Requirements2 { get; }
}