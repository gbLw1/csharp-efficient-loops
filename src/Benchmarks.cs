using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Iterations;

[MemoryDiagnoser(false)]
public class Benchmarks
{
    private static readonly Random Rng = new(42069);

    [Params(1_000, 100_000, 1_000_000)]
    public int Size { get; set; }
    private List<int> _items;

    [GlobalSetup]
    public void Setup()
    {
        _items = Enumerable.Range(0, Size).Select(_ => Rng.Next()).ToList();
    }

    [Benchmark]
    public void For()
    {
        for (var i = 0; i < _items.Count; ++i)
        {
            var item = _items[i];
        }
    }

    [Benchmark]
    public void Foreach()
    {
        foreach (var item in _items)
        {
            var _ = item;
        }
    }

    [Benchmark]
    public void For_Span()
    {
        var asSpan = CollectionsMarshal.AsSpan(_items);
        for (var i = 0; i < asSpan.Length; ++i)
        {
            var item = _items[i];
        }
    }

    [Benchmark]
    public void Foreach_Span()
    {
        foreach (var item in CollectionsMarshal.AsSpan(_items))
        {
            var _ = item;
        }
    }
}