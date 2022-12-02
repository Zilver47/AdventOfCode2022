using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day02 : IChallenge
{
    private readonly string[] _input;

    public Day02(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var rounds = Parse();
        return rounds.Sum(Play);
    }

    private long Play(Tuple<char, char> round)
    {
        if (round.Item1 == round.Item2)
        {
            return Points(round.Item2) + 3;
        }

        if (HaveIWon(round))
        {
            return Points(round.Item2) + 6;
        }

        return Points(round.Item2);
    }

    private bool HaveIWon(Tuple<char, char> round)
    {
        return ((round.Item1 == 'A' && round.Item2 == 'B') ||
            (round.Item1 == 'B' && round.Item2 == 'C') ||
            (round.Item1 == 'C' && round.Item2 == 'A'));
    }

    private int Points(char item)
    {
        return item switch
        {
            'A' => 1,
            'B' => 2,
            'C' => 3,
            _ => 0
        };
    }

    public long Part2()
    {
        var rounds = Parse();
        return rounds.Sum(Play2);
    }

    private long Play2(Tuple<char, char> round)
    {
        return round.Item2 switch
        {
            // lose
            'A' => Points(HowToLose(round.Item1)) + 0,
            // draw
            'B' => Points(round.Item1) + 3,
            // win
            _ => Points(HowToWin(round.Item1)) + 6
        };
    }

    private char HowToLose(char item)
    {
        return item switch
        {
            'A' => 'C',
            'B' => 'A',
            'C' => 'B',
            _ => ' '
        };
    }

    private char HowToWin(char item)
    {
        return item switch
        {
            'A' => 'B',
            'B' => 'C',
            'C' => 'A',
            _ => ' '
        };
    }

    private List<Tuple<char, char>> Parse()
    {
        var rounds = new List<Tuple<char, char>>();
        foreach (var line in _input)
        {
            var splitted = line.Split(' ');
            var opponent = splitted[0].ToCharArray()[0];
            var you = splitted[1].ToCharArray()[0];

            you = Translate(you);

            rounds.Add(new Tuple<char, char>(opponent, you));
        }

        return rounds;
    }

    private static char Translate(char you)
    {
        if (you == 'X') you = 'A';
        if (you == 'Y') you = 'B';
        if (you == 'Z') you = 'C';
        return you;
    }
}