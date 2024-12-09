public class Day09 : Day
{
    public override string Solve1()
    {
        var memory = ReadInitialMemory(out var _);

        var lastFileIndex = memory.FindLastIndex(x => x is not null);
        var firstEmptyIndex = memory.FindIndex(x => x is null);

        while (lastFileIndex > firstEmptyIndex)
        {
            memory[firstEmptyIndex] = memory[lastFileIndex];
            memory[lastFileIndex] = null;

            lastFileIndex = memory.FindLastIndex(x => x is not null);
            firstEmptyIndex = memory.FindIndex(x => x is null);
        }

        return Checksum(memory).ToString();
    }

    public override string Solve2()
    {
        var memory = ReadInitialMemory(out var indexing);
        var filesChecked = new HashSet<int>();

        while (indexing.Any(x => !filesChecked.Contains(x.Id) && x.File is not null))
        {
            var lastFile = indexing.Where(x => !filesChecked.Contains(x.Id) && x.File is not null).LastOrDefault();
            var lastFileIndex = indexing.FindLastIndex(x => !filesChecked.Contains(x.Id) && x.File is not null);

            if (indexing.Any(x => x.File is null && x.Size >= lastFile.Size))
            {
                var firstEmptyFittingBlock = indexing.First(x => x.File is null && x.Size >= lastFile.Size);
                var firstEmptyFittingBlockIndex = indexing.FindIndex(x => x.File is null && x.Size >= lastFile.Size);

                if (firstEmptyFittingBlockIndex > lastFileIndex)
                {
                    filesChecked.Add(lastFile.Id);
                    continue;
                }

                indexing[firstEmptyFittingBlockIndex] = (firstEmptyFittingBlock.Id, firstEmptyFittingBlock.File, firstEmptyFittingBlock.Size - lastFile.Size);
                indexing[lastFileIndex] = (lastFile.Id, null, lastFile.Size);
                indexing.Insert(firstEmptyFittingBlockIndex, lastFile);
            }

            filesChecked.Add(lastFile.Id);
        }

        return Checksum(ToMemory(indexing)).ToString();
    }

    private List<int?> ReadInitialMemory(out List<(int Id, int? File, int Size)> indexing)
    {
        var memory = new List<int?>();
        var inputIndex = 0;
        var memoryIndex = 0;
        var inputNumbers = Input.First().Select(x => (int)char.GetNumericValue(x)).ToArray();

        indexing = new List<(int Position, int? File, int Size)>();

        foreach (var number in inputNumbers)
        {
            if (inputIndex % 2 == 0)
            {
                for (int i = 0; i < number; i++)
                {
                    memory.Add(memoryIndex);
                }

                indexing.Add((Id: inputIndex, File: memoryIndex, Size: number));
                memoryIndex++;
            }
            else
            {
                for (int i = 0; i < number; i++)
                {
                    memory.Add(null);
                }

                indexing.Add((Id: inputIndex, File: null, Size: number));
            }

            inputIndex++;
        }

        return memory;
    }

    private List<int?> ToMemory(List<(int Id, int? File, int Size)> indexing) => indexing.SelectMany((x, i) => Enumerable.Repeat(x.File, x.Size)).ToList();

    private long Checksum(List<int?> memory) => memory.Select((x, i) => x is not null ? x.Value * i : 0L).Sum();
}
