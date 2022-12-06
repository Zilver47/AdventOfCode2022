using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day06 : IChallenge
{
    private readonly string[] _input;

    public Day06(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        const int distinctCharacters = 4;
        var data = _input[0];

        return FindMarker(data, distinctCharacters);
    }

    public long Part2()
    {
        const int distinctCharacters =14;
        var data = _input[0];

        return FindMarker(data, distinctCharacters);
    }

    private static int FindMarker(string data, int distinctCharacters)
    {
        var last = new List<char>();
        for (var index = 0; index < data.Length; index++)
        {
            var character = data[index];

            if (last.Count == distinctCharacters)
            {
                return index;
            }

            while(last.Contains(character))
            {
                last.RemoveAt(0);
            }

            last.Add(character);
        }

        return -1;
    }
}