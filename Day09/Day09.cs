using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day09 : IChallenge
{
    private readonly string[] _input;
    private readonly HashSet<(int X, int y)> _uniquePositionsTail;

    public Day09(string[] input)
    {
        _input = input;
        _uniquePositionsTail = new HashSet<(int x, int y)>();
    }

    public long Part1()
    {
        var knots = new List<Point>
        {
            new() { X = 0, Y = 0 },
            new() { X = 0, Y = 0 }
        };

        _uniquePositionsTail.Add((knots.Last().X, knots.Last().Y));

        ApplyMoves(knots);

        return _uniquePositionsTail.Count;
    }

    public long Part2()
    {
        var knots = new List<Point>
        {
            new() { X = 0, Y = 0 },
            new() { X = 0, Y = 0 },
            new() { X = 0, Y = 0 },
            new() { X = 0, Y = 0 },
            new() { X = 0, Y = 0 },
            new() { X = 0, Y = 0 },
            new() { X = 0, Y = 0 },
            new() { X = 0, Y = 0 },
            new() { X = 0, Y = 0 }
        };

        _uniquePositionsTail.Clear();

        _uniquePositionsTail.Add((knots.Last().X, knots.Last().Y));
        
        ApplyMoves(knots);

        return _uniquePositionsTail.Count;
    }

    private void ApplyMoves(List<Point> knots)
    {
        foreach (var move in _input)
        {
            var splitted = move.Split(' ');
            var numberOfMoves = int.Parse(splitted[1]);
            for (var i = 0; i < numberOfMoves; i++)
            {
                MoveFirst(splitted[0], knots);

                MoveRest(knots);

                RegisterTailPosition(knots.Last());
            }

            //Console.WriteLine($"Head: {positionHead}");
            //Console.WriteLine($"Tail: {positionTail}");
        }
    }

    private void RegisterTailPosition(Point tail)
    {
        if (!_uniquePositionsTail.Contains((tail.X, tail.Y)))
        {
            _uniquePositionsTail.Add((tail.X, tail.Y));
        }
    }

    private static void MoveFirst(string direction, List<Point> knots)
    {
        if (direction == "R")
            knots.First().Y++;
        else if (direction == "U")
            knots.First().Y--;
        else if (direction == "L")
            knots.First().X--;
        else if (direction == "D") 
            knots.First().X++;
    }

    private void MoveRest(List<Point> knots)
    {
        for (var knotIndex = 0; knotIndex < knots.Count - 1; knotIndex++)
        {
            var front = knots[knotIndex];
            var behind = knots[knotIndex + 1];

            if (!front.IsAdjacent(behind))
            {
                MoveCloser(front, behind);
            }
        }
    }

    private void MoveCloser(Point positionHead, Point positionTail)
    {
        if (positionHead.X == positionTail.X + 2)
        {
            positionTail.X++;

            if (positionHead.Y > positionTail.Y)
            {
                positionTail.Y++;
            }
            else if (positionHead.Y < positionTail.Y)
            {
                positionTail.Y--;
            }
        }
        else if (positionHead.X == positionTail.X - 2)
        {
            positionTail.X--;

            if (positionHead.Y > positionTail.Y)
            {
                positionTail.Y++;
            }
            else if (positionHead.Y < positionTail.Y)
            {
                positionTail.Y--;
            }
        }
        else if (positionHead.Y == positionTail.Y + 2)
        {
            positionTail.Y++;

            if (positionHead.X > positionTail.X)
            {
                positionTail.X++;
            }
            else if (positionHead.X < positionTail.X)
            {
                positionTail.X--;
            }
        }
        else if (positionHead.Y == positionTail.Y - 2)
        {
            positionTail.Y--;

            if (positionHead.X > positionTail.X)
            {
                positionTail.X++;
            }
            else if (positionHead.X < positionTail.X)
            {
                positionTail.X--;
            }
        }
    }

    private class Point
    {
        public int X { get; set; }

        public int Y { get; set; }

        public bool IsAdjacent(Point other)
        {
            return (other.X == X || other.X == X - 1 || other.X == X + 1) &&
                   (other.Y == Y || other.Y == Y - 1 || other.Y == Y + 1);
        }

        public override string ToString()
        {
            return $"{X} / {Y}";
        }
    }
}