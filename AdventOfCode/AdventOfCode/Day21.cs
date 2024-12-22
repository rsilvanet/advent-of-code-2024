﻿using System.Numerics;

public class Day21 : Day
{
    public override string Solve1() => PressButtons(amountOfRobots: 2).ToString();

    public override string Solve2() => 0.ToString();

    private long PressButtons(int amountOfRobots)
    {
        var result = 0L;
        var numericRobot = new NumericRobot();
        var directionRobot = new DirectionRobot();

        foreach (var code in Input)
        {
            var numericRobotInput = numericRobot.PressButtons(code);
            var directionRobotInput = directionRobot.PressButtons(numericRobotInput);

            for (var i = 0; i < amountOfRobots - 1; i++)
            {
                directionRobotInput = directionRobot.PressButtons(directionRobotInput);
            }

            result += directionRobot.CountPresses() * int.Parse(code[0..3]);
        }

        return result;
    }

    private abstract class Robot
    {
        private Dictionary<(char, char), char[]> _index;
        private long _presses;

        protected Robot()
        {
            _index = IndexPaths();
        }

        public IEnumerable<char> PressButtons(IEnumerable<char> code)
        {
            _presses = 0;

            var current = 'A';
            var presses = Enumerable.Empty<char>();

            foreach (var next in code)
            {
                var connection = _index[(current, next)];
                presses = presses.Concat(connection);
                current = next;
                _presses += connection.Length;
            }

            return presses;
        }

        public long CountPresses() => _presses;

        private Dictionary<(char, char), char[]> IndexPaths()
        {
            var dictionary = new Dictionary<(char, char), char[]>();

            foreach (var button1 in Pad.Keys)
            {
                foreach (var button2 in Pad.Keys)
                {
                    if (button1 == button2)
                    {
                        dictionary.Add((button1, button2), ['A']);
                    }
                    else
                    {
                        dictionary.Add((button1, button2), FindPath(button1, button2));
                    }
                }
            }

            return dictionary;
        }

        private char[] FindPath(char currentButton, char endButton)
        {
            var possiblePaths = new List<char[]>();
            FindPathRecursive(currentButton, endButton, [], [], possiblePaths);
            return possiblePaths.OrderBy(x => x.Count()).ThenBy(GetPathScore).First();
        }

        private void FindPathRecursive(char currentButton, char endButton, HashSet<char> visited, IEnumerable<char> path, List<char[]> possiblePaths)
        {
            if (currentButton == endButton)
            {
                possiblePaths.Add(path.Append('A').ToArray());
                return;
            }

            var nextButtons = MatrixHelper.FourDirections
                .Select(direction => (Direction: direction, NewPosition: Pad[currentButton] + direction))
                .Where(x => ReversePad.ContainsKey(x.NewPosition) && !visited.Contains(ReversePad[x.NewPosition]))
                .Select(x => (x.Direction, x.NewPosition, Button: ReversePad[x.NewPosition]));

            foreach (var nextButton in nextButtons)
            {
                visited.Add(nextButton.Button);
                FindPathRecursive(ReversePad[nextButton.NewPosition], endButton, visited, path.Append(GetDirectionSign(nextButton.Direction)), possiblePaths);
                visited.Remove(nextButton.Button);
            }
        }

        private char GetDirectionSign(Vector2 direction) => direction switch
        {
            _ when direction == MatrixHelper.Up => '^',
            _ when direction == MatrixHelper.Down => 'v',
            _ when direction == MatrixHelper.Right => '>',
            _ when direction == MatrixHelper.Left => '<',
            _ => throw new NotImplementedException()
        };

        private int GetPathScore(IEnumerable<char> path)
        {
            var score = path.Count();

            if (path.ElementAt(0) == '<')
            {
                score--;
            }

            for (int i = 1; i < path.Count(); i++)
            {
                if (path.ElementAt(i) == path.ElementAt(i - 1))
                {
                    score--;
                }
            }

            return score;
        }

        protected abstract Dictionary<char, Vector2> Pad { get; }
        protected abstract Dictionary<Vector2, char> ReversePad { get; }
        protected abstract bool Optimized { get; }
    }

    private class NumericRobot : Robot
    {
        protected override Dictionary<char, Vector2> Pad { get; } = _pad;
        protected override Dictionary<Vector2, char> ReversePad { get; } = _reversePad;
        protected override bool Optimized { get; } = false;

        private readonly static Dictionary<char, Vector2> _pad = new Dictionary<char, Vector2>()
        {
            { '7', new (0, 0) }, { '8', new (1, 0) }, { '9', new (2, 0) },
            { '4', new (0, 1) }, { '5', new (1, 1) }, { '6', new (2, 1) },
            { '1', new (0, 2) }, { '2', new (1, 2) }, { '3', new (2, 2) },
                                 { '0', new (1, 3) }, { 'A', new (2, 3) },
        };

        private readonly static Dictionary<Vector2, char> _reversePad = _pad.ToDictionary(x => x.Value, x => x.Key);
    }

    private class DirectionRobot : Robot
    {
        protected override Dictionary<char, Vector2> Pad { get; } = _pad;
        protected override Dictionary<Vector2, char> ReversePad { get; } = _reversePad;
        protected override bool Optimized { get; } = false;

        private readonly static Dictionary<char, Vector2> _pad = new Dictionary<char, Vector2>()
        {
                                 { '^', new (1, 0) }, { 'A', new (2, 0) },
            { '<', new (0, 1) }, { 'v', new (1, 1) }, { '>', new (2, 1) },
        };

        private readonly static Dictionary<Vector2, char> _reversePad = _pad.ToDictionary(x => x.Value, x => x.Key);
    }
}