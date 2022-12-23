using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day21 : IChallenge
{
    private readonly string[] _input;

    public Day21(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var monkeys = Parse();

        var results = new Dictionary<string, long>();
        foreach (var monkey in monkeys.OfType<SimpleYell>())
        {
            results.Add(monkey.Name, monkey.Evaluate(results));
        }

        var mathMonkeys = monkeys.OfType<MathYell>().ToList();
        return Solve(mathMonkeys, results);
    }

    public long Part2()
    {
        // input changed
        var monkeys = Parse();

        var results = new Dictionary<string, long>();
        foreach (var monkey in monkeys.OfType<SimpleYell>())
        {
            results.Add(monkey.Name, monkey.Evaluate(results));
        }

        results["humn"] = 0;

        var increment = 100000000000L;
        var mathMonkeys = monkeys.OfType<MathYell>().ToList();
        while (true)
        {
            results["humn"] += increment;

            var result = Solve(mathMonkeys, new Dictionary<string, long>(results));
            if (result == 0) break;

            if (result < 0)
            {
                results["humn"] -= increment;
                increment /= 10;
            }
        }
        
        return results["humn"];
    }

    private static long Solve(List<MathYell> monkeys, Dictionary<string, long> results)
    {
        var queue = new Queue<MathYell>(monkeys);
        while (queue.Count > 0)
        {
            var monkey = queue.Dequeue();
            if (monkey.CanSolve(results))
            {
                results.Add(monkey.Name, monkey.Evaluate(results));
            }
            else
            {
                queue.Enqueue(monkey);
            }
        }

        return results["root"];
    }

    private List<IYell> Parse()
    {
        var result = new List<IYell>();
        foreach (var line in _input)
        {
            if (line.Length > 11)
            {
                result.Add( new MathYell(line));
            }
            else
            {
                result.Add(new SimpleYell(line));
            }
        }
        return result;
    }

    private interface IYell 
    {
        public string Name { get; }

        public IEnumerable<string> Parts { get; }

        public long Evaluate(Dictionary<string, long> options);
        bool CanSolve(Dictionary<string, long> results);
    }

    private class SimpleYell : IYell
    {
        private readonly int _value;

        public SimpleYell(string line)
        {
            var parts = line.Split(':');
            Name = parts[0];
            _value = int.Parse(parts[1]);
        }

        public string Name { get; }
        public IEnumerable<string> Parts => Array.Empty<string>();
        public long Evaluate(Dictionary<string, long> options) => _value;

        public bool CanSolve(Dictionary<string, long> results) => true;
    }

    private class MathYell : IYell
    {
        private readonly string _operator;

        public MathYell(string line)
        {
            var parts = line.Split(':');
            Name = parts[0];

            var x = parts[1].TrimStart().Split(' ');
            Parts = new List<string>
            {
                x[0],
                x[2]
            };

            _operator = x[1];
        }

        public string Name { get; }
        public IEnumerable<string> Parts { get; }
        public long Evaluate(Dictionary<string, long> options)
        {
            return _operator switch
            {
                "+" => options[Parts.First()] + options[Parts.Last()],
                "-" => options[Parts.First()] - options[Parts.Last()],
                "*" => options[Parts.First()] * options[Parts.Last()],
                _ => options[Parts.First()] / options[Parts.Last()]
            };
        }

        public bool CanSolve(Dictionary<string, long> results)
        {
            return Parts.All(results.ContainsKey);
        }
    }
}