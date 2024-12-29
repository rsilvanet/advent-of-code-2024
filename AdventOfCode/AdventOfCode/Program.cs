var days = AppDomain.CurrentDomain
    .GetAssemblies()
    .SelectMany(x => x.GetTypes())
    .Where(x => typeof(Day).IsAssignableFrom(x) && !x.IsAbstract)
    .OrderBy(x => x.Name)
    .Select(Activator.CreateInstance)
    .OfType<Day>();

var stopwatch = new System.Diagnostics.Stopwatch();

foreach (var day in days)
{
    stopwatch.Restart();
    Console.WriteLine($"{day.GetType().Name} - Part 1: {day.Solve1()} in {stopwatch.ElapsedMilliseconds}ms");

    stopwatch.Restart();
    Console.WriteLine($"{day.GetType().Name} - Part 2: {day.Solve2()} in {stopwatch.ElapsedMilliseconds}ms");
}