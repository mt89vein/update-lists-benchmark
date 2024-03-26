using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Engines;

namespace LinqJoinBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<JoinBenchmarks>();

            Console.ReadKey();
        }
    }

    public class Test : IEquatable<Test>
    {
        public Guid Id { get; }

        public Guid G { get; set; } = Guid.NewGuid();

        public Test(Guid id)
        {
            Id = id;
        }

        public bool Equals(Test other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Test)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }

    [SimpleJob(RuntimeMoniker.Net80)]
    [MemoryDiagnoser]
    public class JoinBenchmarks
    {
        private List<Test> _existingItems;
        private List<Test> _newItems;

        [Params(10, 100, 1000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            _existingItems = new List<Test>(N);
            _newItems = new List<Test>(N);

            for (var i = 0; i < N; i++)
            {
                _existingItems.Add(new Test(Guid.NewGuid()));
            }

            _newItems = _existingItems.ToList();
            _newItems.Shuffle();
            var toRemove = N / 2; // delete half of them

            for (var i = 0; i < toRemove; i++)
            {
                _newItems.Add(new Test(Guid.NewGuid()));
            }
            _newItems.Shuffle();
        }

        [Benchmark]
        public void FullOuterLinqJoin()
        {
            var allIds = _existingItems.Concat(_newItems).Select(x => x.Id).Distinct();

            var joined =
                from id in allIds
                join existing in _newItems on id equals existing.Id into joinedA
                from existing in joinedA.DefaultIfEmpty()
                join newOne in _existingItems on id equals newOne.Id into joinedB
                from newOne in joinedB.DefaultIfEmpty()
                select (existing, newOne);

            var finalItems = new List<Test>(_existingItems.Count);

            foreach (var (existing, newOne) in joined)
            {
                if (existing is null && newOne is not null)
                {
                    finalItems.Add(newOne);

                    continue;
                }

                if (existing is not null && newOne is not null)
                {
                    finalItems.Add(existing);
                    existing.G = newOne.G; // simulate update
                }

                if (existing is not null && newOne is null)
                {
                    // mark as delete here

                    continue;
                }
            }

            finalItems.Consume(new Consumer());
        }

        [Benchmark]
        public void ShortLinqs()
        {
            _existingItems.AddRange(_newItems.Except(_existingItems));
            _existingItems.RemoveAll(item => !_newItems.Contains(item));
            _existingItems.ForEach(existing =>
            {
                var found = _newItems.Find(i => i.Id == existing.Id);
                if (found is not null)
                {
                    existing.G = found.G;
                }
            });

            _existingItems.Consume(new Consumer());
        }

        // found from internet, very slow approach :)
        // [Benchmark]
        // public void LinqSets()
        // {
        //     _existingItems.AddRange(_newItems.Except(_existingItems));
        //     _existingItems.RemoveAll(item => !_newItems.Contains(item));
        //
        //     var a = from ele in _existingItems
        //         let alleles = _existingItems.Union(_newItems)
        //         let deletes = alleles.Except(_existingItems)
        //         let adds = alleles.Except(_newItems)
        //         let updates = _existingItems.Intersect(_newItems)
        //         select (deletes, adds, updates);
        //
        //     foreach (var (deletes, adds, updates) in a)
        //     {
        //         _existingItems.RemoveAll(item => deletes.Contains(item));
        //         _existingItems.AddRange(adds);
        //         foreach (var update in updates)
        //         {
        //             var existing = _existingItems.Find(x => x.Id == update.Id);
        //
        //             if (existing is not null)
        //             {
        //                 existing.G = update.G;
        //             }
        //         }
        //     }
        //
        //     _existingItems.Consume(new Consumer());
        // }

        [Benchmark]
        public void ManualForeach()
        {
            foreach (var newItem in _newItems)
            {
                if (!_existingItems.Contains(newItem))
                {
                    _existingItems.Add(newItem);
                }

            }

            foreach (var existingItem in _existingItems)
            {
                if (!_newItems.Contains(existingItem))
                {
                    _existingItems.Remove(existingItem);
                }
                else
                {
                    var found = _newItems.Find(x => x.Id == existingItem.Id);

                    if (found is not null)
                    {
                        existingItem.G = found.G;
                    }
                }
            }

            _existingItems.Consume(new Consumer());
        }

        [Benchmark]
        public void ManualForWithoutLinq()
        {
            for (var i = 0; i < _newItems.Count; i++)
            {
                var newItem = _newItems[i];
                if (!_existingItems.Contains(newItem))
                {
                    _existingItems.Add(newItem);
                }
            }

            for (var i = 0; i < _existingItems.Count; i++)
            {
                var existingItem = _existingItems[i];
                if (!_newItems.Contains(existingItem))
                {
                    _existingItems.Remove(existingItem);
                }
                else
                {
                    var found = _newItems.Find(x => x.Id == existingItem.Id);

                    if (found is not null)
                    {
                        existingItem.G = found.G;
                    }
                }
            }

            _existingItems.Consume(new Consumer());
        }

    }

    public static class Shuffler
    {
        private static readonly Random _random = new Random();

        public static void Shuffle<T>(this IList<T> arr)
        {
            var n = arr.Count;
            while (n > 1)
            {
                n--;
                var k = _random.Next(n + 1);
                var value = arr[k];
                arr[k] = arr[n];
                arr[n] = value;
            }
        }
    }
}
