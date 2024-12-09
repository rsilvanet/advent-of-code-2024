public class Day09 : Day
{
    public override string Solve1()
    {
        var memory = SpreadMemory(GetMemoryIndexing());
        var nullIndexes = new Queue<int>(memory.Select((x, i) => (Value: x, Index: i)).Where(x => x.Value is null).Select(x => x.Index));
        var reversedNonNullIndexes = memory.Select((x, i) => (Value: x, Index: i)).Where(x => x.Value is not null).Reverse();

        foreach (var item in reversedNonNullIndexes)
        {
            if (!nullIndexes.TryDequeue(out var firstNullIndex) || firstNullIndex > item.Index)
            {
                break;
            }

            memory[item.Index] = null;
            memory[firstNullIndex] = item.Value;
        }

        return Checksum(memory).ToString();
    }

    public override string Solve2()
    {
        var filesChecked = new HashSet<int>();
        var memoryIndexing = GetMemoryIndexing().ToList();
        var lastFileIndex = memoryIndexing.FindLastIndex(x => !filesChecked.Contains(x.MemoryIndex) && x.FileIndex is not null);

        while (lastFileIndex >= 0)
        {
            var lastFile = memoryIndexing[lastFileIndex];
            var firstEmptyIndex = memoryIndexing.FindIndex(x => x.FileIndex is null && x.Size >= lastFile.Size);

            if (firstEmptyIndex >= 0 && lastFileIndex > firstEmptyIndex)
            {
                var firstEmptyBlock = memoryIndexing[firstEmptyIndex];

                memoryIndexing[firstEmptyIndex] = (firstEmptyBlock.MemoryIndex, firstEmptyBlock.FileIndex, firstEmptyBlock.Size - lastFile.Size);
                memoryIndexing[lastFileIndex] = (lastFile.MemoryIndex, null, lastFile.Size);
                memoryIndexing.Insert(firstEmptyIndex, lastFile);
            }

            filesChecked.Add(lastFile.MemoryIndex);
            lastFileIndex = memoryIndexing.FindLastIndex(x => !filesChecked.Contains(x.MemoryIndex) && x.FileIndex is not null);
        }

        return Checksum(SpreadMemory(memoryIndexing)).ToString();
    }

    private IEnumerable<(int MemoryIndex, int? FileIndex, int Size)> GetMemoryIndexing() => Input.First()
        .Select((size, index) => (
            MemoryIndex: index,
            FileIndex: index % 2 == 0 ? index / 2 : (int?)null,
            Size: (int)char.GetNumericValue(size)
        ));

    private int?[] SpreadMemory(IEnumerable<(int MemoryIndex, int? FileIndex, int Size)> indexing) => indexing
        .SelectMany(x => Enumerable.Repeat(x.FileIndex, x.Size))
        .ToArray();

    private long Checksum(IEnumerable<int?> memory) => memory
        .Select((x, i) => x is not null ? x.Value * i : 0L)
        .Sum();
}
