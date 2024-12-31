using System.Numerics;

public class Day21 : Day
{
    public override string Solve1() => PressButtons(amountOfPads: 2).ToString();

    public override string Solve2() => PressButtons(amountOfPads: 25).ToString();

    private long PressButtons(int amountOfPads)
    {
        var complexity = 0L;

        foreach (var code in Input)
        {
            var manualKeypad = new DirectionalKeypad();
            var directionalKeypad = new DirectionalKeypad();
            var nextKeypad = manualKeypad;

            for (int i = 0; i < amountOfPads; i++)
            {
                directionalKeypad = new DirectionalKeypad(nextKeypad);
                nextKeypad = directionalKeypad;
            }

            var count = 0L;
            var numericKeypad = new NumericKeypad(directionalKeypad);

            foreach (var button in code)
            {
                count = numericKeypad.Press(button, count);
            }

            complexity += count * int.Parse(code[0..3]);
        }

        return complexity;
    }

    private abstract class Keypad
    {
        private char _currentButton;
        private readonly Dictionary<(char, char), char[]> _index;
        private readonly Dictionary<string, long> _cache;
        private readonly Keypad? _nextKeypad;

        public Keypad()
        {
            _currentButton = 'A';
            _index = IndexPaths();
            _cache = new();
        }

        public Keypad(Keypad next) : this()
        {
            _nextKeypad = next;
        }

        public long Press(char button, long count)
        {
            var cacheKey = $"{_currentButton}|{button}";
            var nextKeypadButtons = _index[(_currentButton, button)];

            _currentButton = button;

            if (_cache.TryGetValue(cacheKey, out var cached))
            {
                return cached + count;
            }

            var previousCount = count;

            if (_nextKeypad is null)
            {
                count++;
            }
            else
            {
                foreach (var nextKeypadButton in nextKeypadButtons)
                {
                    count = _nextKeypad.Press(nextKeypadButton, count);
                }
            }

            _cache[cacheKey] = count - previousCount;

            return count;
        }

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
    }

    private class NumericKeypad : Keypad
    {
        public NumericKeypad(Keypad next) : base(next) { }

        protected override Dictionary<char, Vector2> Pad { get; } = _pad;
        protected override Dictionary<Vector2, char> ReversePad { get; } = _reversePad;

        private readonly static Dictionary<char, Vector2> _pad = new Dictionary<char, Vector2>()
        {
            { '7', new (0, 0) }, { '8', new (1, 0) }, { '9', new (2, 0) },
            { '4', new (0, 1) }, { '5', new (1, 1) }, { '6', new (2, 1) },
            { '1', new (0, 2) }, { '2', new (1, 2) }, { '3', new (2, 2) },
                                 { '0', new (1, 3) }, { 'A', new (2, 3) },
        };

        private readonly static Dictionary<Vector2, char> _reversePad = _pad.ToDictionary(x => x.Value, x => x.Key);
    }

    private class DirectionalKeypad : Keypad
    {
        public DirectionalKeypad() : base() { }
        public DirectionalKeypad(Keypad next) : base(next) { }

        protected override Dictionary<char, Vector2> Pad { get; } = _pad;
        protected override Dictionary<Vector2, char> ReversePad { get; } = _reversePad;

        private readonly static Dictionary<char, Vector2> _pad = new Dictionary<char, Vector2>()
        {
                                 { '^', new (1, 0) }, { 'A', new (2, 0) },
            { '<', new (0, 1) }, { 'v', new (1, 1) }, { '>', new (2, 1) },
        };

        private readonly static Dictionary<Vector2, char> _reversePad = _pad.ToDictionary(x => x.Value, x => x.Key);
    }
}