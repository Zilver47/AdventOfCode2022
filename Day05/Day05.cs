using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

internal class Day05 : IChallenge
{
    private readonly string[] _input;
    private List<Move> _moves;

    public Day05(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var stacks = Parse();

        foreach (var move in _moves)
        {
            ApplyMove(stacks, move);
        }

        foreach (var stack in stacks)
        {
            Console.Write(stack.Pop());
        }

        return -1;
    }

    private void ApplyMove(List<Stack<string>> stacks, Move move)
    {
        for (var i = 0; i < move.NumberOf; i++)
        {
            var crate = stacks[move.From - 1].Pop();
            stacks[move.To - 1].Push(crate);
        }
    }

    public long Part2()
    {
        var stacks = Parse();

        foreach (var move in _moves)
        {
            ApplyMove2(stacks, move);
        }

        foreach (var stack in stacks)
        {
            Console.Write(stack.Pop());
        }

        return -1;
    }

    private void ApplyMove2(List<Stack<string>> stacks, Move move)
    {
        var crates = new List<string>();
        for (var i = 0; i < move.NumberOf; i++)
        {
            var crate = stacks[move.From - 1].Pop();
            crates.Add(crate);
        }

        for (var i = crates.Count - 1; i >= 0; i--)
        {
            stacks[move.To - 1].Push(crates[i]);
        }
    }

    private List<Stack<string>> Parse()
    {
        var result = new List<Stack<string>>();
        _moves = new List<Move>();

        var stackNumberLineIndex = 0;
        for (var i = 0; i < _input.Length; i++)
        {
            if (_input[i].StartsWith(" 1"))
            {
                stackNumberLineIndex = i;
                break;
            }
        }

        var numberOfStacks = int.Parse(_input[stackNumberLineIndex].Trim()[^1].ToString());
        
        for (var i = 0; i < numberOfStacks; i++)
        {
            result.Add(new Stack<string>());
        }

        for (var j = stackNumberLineIndex - 1; j>= 0 ; j--)
        {
            var lineIndex = 0;
            for (var i = 0; i < _input[j].Length; i += 4)
            {
                var crate = _input[j].Substring(i, 3);
                if (!string.IsNullOrWhiteSpace(crate))
                {
                    crate = crate.Replace("[", string.Empty)
                        .Replace("]", string.Empty);
                    result[lineIndex].Push(crate);
                }

                lineIndex++;
            }
        }

        for (var i = stackNumberLineIndex + 2; i < _input.Length; i++)
        {
            _moves.Add(new Move(_input[i]));
        }

        return result;
    }

    private class Move
    {
        public Move(string input)
        {
            var splitted = input.Split(' ');
            NumberOf = int.Parse(splitted[1]);
            From = int.Parse(splitted[3]);
            To = int.Parse(splitted[5]);
        }

        public int NumberOf { get; set; }

        public int From { get; set; }

        public int To { get; set; }
    }
}