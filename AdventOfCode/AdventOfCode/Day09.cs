using System.Linq;
using System.Net.Http.Headers;

public class Day09 : Day
{
    public override string Solve1()
    {
        var memory = ToMemory(GetMemoryIndexes()).ToArray();
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
        var memoryIndexes = GetMemoryIndexes().ToList();

        while (memoryIndexes.Any(x => !filesChecked.Contains(x.MemoryIndex) && x.FileIndex is not null))
        {
            var lastFile = memoryIndexes.Where(x => !filesChecked.Contains(x.MemoryIndex) && x.FileIndex is not null).LastOrDefault();
            var lastFileIndex = memoryIndexes.FindLastIndex(x => !filesChecked.Contains(x.MemoryIndex) && x.FileIndex is not null);

            if (memoryIndexes.Any(x => x.FileIndex is null && x.Size >= lastFile.Size))
            {
                var firstEmptyFittingBlock = memoryIndexes.First(x => x.FileIndex is null && x.Size >= lastFile.Size);
                var firstEmptyFittingBlockIndex = memoryIndexes.FindIndex(x => x.FileIndex is null && x.Size >= lastFile.Size);

                if (firstEmptyFittingBlockIndex > lastFileIndex)
                {
                    filesChecked.Add(lastFile.MemoryIndex);
                    continue;
                }

                memoryIndexes[firstEmptyFittingBlockIndex] = (firstEmptyFittingBlock.MemoryIndex, firstEmptyFittingBlock.FileIndex, firstEmptyFittingBlock.Size - lastFile.Size);
                memoryIndexes[lastFileIndex] = (lastFile.MemoryIndex, null, lastFile.Size);
                memoryIndexes.Insert(firstEmptyFittingBlockIndex, lastFile);
            }

            filesChecked.Add(lastFile.MemoryIndex);
        }

        return Checksum(ToMemory(memoryIndexes)).ToString();
    }

    private IEnumerable<(int MemoryIndex, int? FileIndex, int Size)> GetMemoryIndexes() => Input.First().Select((size, index) => (
        MemoryIndex: index,
        FileIndex: index % 2 == 0 ? index / 2 : (int?)null,
        Size: (int)char.GetNumericValue(size)
    ));

    private IEnumerable<int?> ToMemory(IEnumerable<(int Id, int? File, int Size)> indexing) => indexing.SelectMany((x, i) => Enumerable.Repeat(x.File, x.Size));

    private long Checksum(IEnumerable<int?> memory) => memory.Select((x, i) => x is not null ? x.Value * i : 0L).Sum();
}
