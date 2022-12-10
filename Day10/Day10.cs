using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day10 : IChallenge
{
    private readonly string[] _input;

    public Day10(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var signalStrength = new List<long>
        {
            0, 0, 0,0, 0,0
        };
        var x = 1;
        var cycle = 1;
        foreach (var instruction in _input)
        {
            var splitted = instruction.Split(' ');
            if (splitted[0] == "addx")
            {
                var v = int.Parse(splitted[1]);

                cycle++;
                Check(x, cycle, signalStrength);

                cycle++;
                x += v;
            }
            else
            {
                cycle++;
            }

            Check(x, cycle, signalStrength);
        } 
        
        return signalStrength.Sum();
    }

    private static void Check(int x, int cycle, List<long> signalStrength)
    {
        if (cycle == 20)
        {
            signalStrength[0] = x * 20;
        }
        else if (cycle == 60)
        {
            signalStrength[1] = x * 60;
        }
        else if (cycle == 100)
        {
            signalStrength[2] = x * 100;
        }
        else if (cycle == 140)
        {
            signalStrength[3] = x * 140;
        }
        else if (cycle == 180)
        {
            signalStrength[4] = x * 180;
        }
        else if (cycle == 220)
        {
            signalStrength[5] = x * 220;
        }
    }

    public long Part2()
    {
        Console.WriteLine();
        
        var x = 1;
        var cycle = 1;
        foreach (var instruction in _input)
        {
            var splitted = instruction.Split(' ');
            if (splitted[0] == "addx")
            {
                var v = int.Parse(splitted[1]);

                DrawPixel(x, cycle);
                cycle++;

                DrawPixel(x, cycle);
                cycle++;

                x += v;
            }
            else
            {
                DrawPixel(x, cycle);
                cycle++;
            }

        }

        return 0;
    }

    private void DrawPixel(int x, int cycle)
    {
        var position = cycle % 40 - 1;

        CheckForNewLine(position);

        if (position == x - 1 || position == x || position == x + 1)
        {
            Console.Write("#");
        }
        else
        {
            Console.Write(".");
        }

    }

    private static void CheckForNewLine(int cycle)
    {
        if (cycle == 0)
        {
            Console.WriteLine();
        }
    }
}