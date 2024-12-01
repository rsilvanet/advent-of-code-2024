public class Day01 : Day
{
    public override string Solve1() => List1.Select((itemList1, i) => Math.Abs(itemList1 - List2.ElementAt(i))).Sum().ToString();

    public override string Solve2() => List1.Sum(itemList1 => itemList1 * List2.Count(itemList2 => itemList2 == itemList1)).ToString();

    public Day01()
    {
        var parsedInput = Input.Select(x => x.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse));
        List1 = parsedInput.Select(x => x.ElementAt(0)).Order();
        List2 = parsedInput.Select(x => x.ElementAt(1)).Order();
    }

    public IEnumerable<int> List1 { get; }
    public IEnumerable<int> List2 { get; }
}
