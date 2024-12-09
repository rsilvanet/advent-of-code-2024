public class Day09 : Day
{
    public override string Solve1()
    {
        var memory = SpreadMemory(GetMemoryIndexing()).ToArray();
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

        while (memoryIndexing.Any(x => !filesChecked.Contains(x.MemoryIndex) && x.FileIndex is not null))
        {
            var lastFile = memoryIndexing.Where(x => !filesChecked.Contains(x.MemoryIndex) && x.FileIndex is not null).LastOrDefault();
            var lastFileIndex = memoryIndexing.FindLastIndex(x => !filesChecked.Contains(x.MemoryIndex) && x.FileIndex is not null);

            if (memoryIndexing.Any(x => x.FileIndex is null && x.Size >= lastFile.Size))
            {
                var firstEmptyFittingBlock = memoryIndexing.First(x => x.FileIndex is null && x.Size >= lastFile.Size);
                var firstEmptyFittingBlockIndex = memoryIndexing.FindIndex(x => x.FileIndex is null && x.Size >= lastFile.Size);

                if (firstEmptyFittingBlockIndex > lastFileIndex)
                {
                    filesChecked.Add(lastFile.MemoryIndex);
                    continue;
                }

                memoryIndexing[firstEmptyFittingBlockIndex] = (firstEmptyFittingBlock.MemoryIndex, firstEmptyFittingBlock.FileIndex, firstEmptyFittingBlock.Size - lastFile.Size);
                memoryIndexing[lastFileIndex] = (lastFile.MemoryIndex, null, lastFile.Size);
                memoryIndexing.Insert(firstEmptyFittingBlockIndex, lastFile);
            }

            filesChecked.Add(lastFile.MemoryIndex);
        }

        return Checksum(SpreadMemory(memoryIndexing)).ToString();
    }

    private IEnumerable<(int MemoryIndex, int? FileIndex, int Size)> GetMemoryIndexing() => Input.First()
        .Select((size, index) => (
            MemoryIndex: index,
            FileIndex: index % 2 == 0 ? index / 2 : (int?)null,
            Size: (int)char.GetNumericValue(size)
        ));

    private IEnumerable<int?> SpreadMemory(IEnumerable<(int Id, int? File, int Size)> indexing) => indexing.SelectMany(x => Enumerable.Repeat(x.File, x.Size));

    private long Checksum(IEnumerable<int?> memory) => memory.Select((x, i) => x is not null ? x.Value * i : 0L).Sum();
}
