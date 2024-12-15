using System.Numerics;
using System.Text.RegularExpressions;

public class Day14 : Day
{
    public override string Solve1()
    {
        for (int seconds = 0; seconds < 100; seconds++)
        {
            foreach (var robot in Robots)
            {
                robot.Move(Border);
            }
        }

        var topLeftCount = Robots.Where(r => r.Position.X < Center.X && r.Position.Y < Center.Y).Count();
        var topRightCount = Robots.Where(r => r.Position.X > Center.X / 2 && r.Position.Y < Center.Y).Count();
        var bottomLeftCount = Robots.Where(r => r.Position.X < Center.X / 2 && r.Position.Y > Center.Y).Count();
        var bottomRightCount = Robots.Where(r => r.Position.X > Center.X / 2 && r.Position.Y > Center.Y).Count();

        return (topLeftCount * bottomLeftCount * topRightCount * bottomRightCount).ToString();
    }

    public override string Solve2()
    {
        ResetRobots();

        int seconds = 0;
        var diagonal = Enumerable.Range(1, 6).Select(x => new Vector2(x, -x)).ToArray();

        while (true)
        {
            seconds++;

            foreach (var robot in Robots)
            {
                robot.Move(Border);
            }

            var hashset = Robots.Select(r => r.Position).ToHashSet();

            foreach (var position in hashset)
            {
                if (diagonal.All(d => hashset.Contains(position + d)))
                {
                    return seconds.ToString();
                }
            }
        }
    }

    public Day14()
    {
        Border = new(100, 102);
        Center = new(Border.X / 2, Border.Y / 2);
        ResetRobots();
    }

    private void ResetRobots()
    {
        var regex = new Regex("p=(-?\\d+),(-?\\d+)\\s+v=(-?\\d+),(-?\\d+)");

        Robots = new List<Robot>();

        foreach (var line in Input)
        {
            var matches = regex.Match(line);

            Robots.Add(new Robot(
                position: new Vector2(int.Parse(matches.Groups[1].Value), int.Parse(matches.Groups[2].Value)),
                speed: new Vector2(int.Parse(matches.Groups[3].Value), int.Parse(matches.Groups[4].Value))
            ));
        }
    }

    private Vector2 Border { get; }
    private Vector2 Center { get; }
    private List<Robot> Robots { get; set; } = [];

    private class Robot(Vector2 position, Vector2 speed)
    {
        public Vector2 Position { get; private set; } = position;
        public Vector2 Speed { get; private set; } = speed;

        public void Move(Vector2 border)
        {
            Position += Speed;

            if (Position.X > border.X)
            {
                Position = new Vector2(Position.X - border.X - 1, Position.Y);
            }
            else if (Position.X < 0)
            {
                Position = new Vector2(border.X + Position.X + 1, Position.Y);
            }

            if (Position.Y > border.Y)
            {
                Position = new Vector2(Position.X, Position.Y - border.Y - 1);
            }
            else if (Position.Y < 0)
            {
                Position = new Vector2(Position.X, border.Y + Position.Y + 1);
            }
        }
    }
}

