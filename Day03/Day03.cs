using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day03 : IChallenge
{
    private readonly string[] _input;

    public Day03(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var lines = _input.ToList();

        long score = lines.Sum(l => Priority(FindCommon(l)));

        return score;
    }

    private static int Priority(char value)
    {
        if (char.IsLower(value))
        {
            return value - 96;
        }

        return value - 38;
    }

    private static char FindCommon(string value)
    {
        var first = value[..(value.Length / 2)];
        var second = value[(value.Length / 2)..];
        var intersect = first.Intersect(second);
        return intersect.First();
    }

    public long Part2()
    {
        var lines = _input.ToList();

        var group = new List<string>();
        long score = 0;
        for (var i = 0; i < lines.Count; i++)
        {
            group.Add(lines[i]);

            if ((i+1) % 3 == 0)
            {
                score += Priority(FindCommon2(group));
                group.Clear();
            }
        }

        return score;
    }

    private static char FindCommon2(IReadOnlyList<string> value)
    {
        var intersect = value[0].Intersect(value[1]).Intersect(value[2]);
        return intersect.First();
    }
}