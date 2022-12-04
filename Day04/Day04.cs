using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

internal class Day04 : IChallenge
{
    private readonly string[] _input;

    public Day04(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var ranges = ParseInput();

        return ranges.Sum(IsScore);
    }

    private static int IsScore(Tuple<Range, Range> range)
    {
        if (range.Item1.Contains(range.Item2) ||
            range.Item2.Contains(range.Item1))
        {
            return 1;
        }

        return 0;
    }

    private List<Tuple<Range, Range>> ParseInput()
    {
        var result = new List<Tuple<Range, Range>>();
        foreach (var line in _input)
        {
            var splitted = line.Split(',');
            result.Add(new Tuple<Range, Range>(new Range(splitted[0]), new Range(splitted[1])));
        }
        return result;
    }

    public long Part2()
    {
        var ranges = ParseInput();

        return ranges.Sum(IsScore2);
    }

    private static int IsScore2(Tuple<Range, Range> range)
    {
        if (range.Item1.ContainsPartial(range.Item2) ||
            range.Item2.ContainsPartial(range.Item1))
        {
            return 1;
        }

        return 0;
    }

    private class Range
    {
        public Range(string input)
        {
            var splitted = input.Split('-');
            Min = int.Parse(splitted[0]);
            Max = int.Parse(splitted[1]);
        }

        private int Max { get; }

        private int Min { get; }

        public bool Contains(Range other)
        {
            return other.Min >= Min && other.Max <= Max;
        }

        public bool ContainsPartial(Range other)
        {
            if (other.Min >= Min && other.Min <= Max) return true;
            if (other.Max >= Min && other.Max <= Max) return true;
            return false;
        }
    }
}
