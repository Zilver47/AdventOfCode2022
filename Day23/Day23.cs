using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day23 : IChallenge
{
    private readonly string[] _input;

    public Day23(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var elves = Parse(_input.ToList()).ToList();

        for (var round = 0; round < 10; round++)
        {
            var positions = elves.Select(e => e.Position).ToList();

            Parallel.ForEach(elves, elf =>
            {
                elf.ConsiderPosition(positions);
            });

            var consideredPositions = elves.Select(x => x.ConsideredPosition).ToList();

            Parallel.ForEach(elves, elf =>
            {
                elf.Move(consideredPositions);
            });
        }

        var position = elves.Select(e => e.Position);
        var minRow = position.Min(p => p.Row);
        var maxRow = position.Max(p => p.Row);
        var minColumn = position.Min(p => p.Column);
        var maxColumn = position.Max(p => p.Column);

        var rows = maxRow - minRow + 1;
        var cols = maxColumn - minColumn + 1;

        return rows * cols - elves.Count;
    }

    public long Part2()
    {
        var elves = Parse(_input.ToList()).ToList();
        
        var result = 0;
        for (var round = 0; round < 1056; round++)
        {
            var positions = elves.Select(e => e.Position).ToList();

            Parallel.ForEach(elves, elf =>
            {
                elf.ConsiderPosition(positions);
            });

            if (elves.All(e => e.Satisfied()))
            {
                result = round + 1;
                break;
            }

            var consideredPositions = elves.Select(x => x.ConsideredPosition).ToList();

            Parallel.ForEach(elves, elf =>
            {
                elf.Move(consideredPositions);
            });
        }
        
        return result;
    }

    private class Elf
    {
        private int _initialOrientation;

        public Position ConsideredPosition { get; private set; }

        public Position Position { get; private set; }

        public Elf(int row, int column)
        {
            Position = new Position(row, column);
            _initialOrientation = 0;
        }

        public void ConsiderPosition(ICollection<Position> positions)
        {
            var possiblePositions = GetPossiblePositions(Position.Row, Position.Column, positions);
            if (possiblePositions.All(v => !v))
            {
                ConsideredPosition = Position;
                _initialOrientation++;
                return;
            }

            var direction = -1;
            for (var i = _initialOrientation; i < _initialOrientation + 4; i++)
            {
                var isOccupied = GetPositionsToConsider(possiblePositions.ToList(), i % 4);
                if (!isOccupied)
                {
                    direction = i % 4;
                    break;
                }
            }

            _initialOrientation++;

            ConsideredPosition = direction switch
            {
                -1 => ConsideredPosition = Position,
                0 => ConsideredPosition = Position with { Row = Position.Row - 1 },
                1 => ConsideredPosition = Position with { Row = Position.Row + 1 },
                2 => ConsideredPosition = Position with { Column = Position.Column - 1 },
                3 => ConsideredPosition = Position with { Column = Position.Column + 1 }
            };
        }

        private static IList<bool> GetPossiblePositions(int row, int column, ICollection<Position> positions)
        {
            var newPositions = new List<Position>
            {
                new(row - 1, column - 1),
                new(row - 1, column),
                new(row - 1, column + 1),
                new(row, column - 1),
                new(row, column + 1),
                new(row+1, column - 1),
                new(row + 1, column ),
                new(row+1, column + 1)
            };

            return newPositions.Select(positions.Contains).ToList();
        }

        private static bool GetPositionsToConsider(IList<bool> positions, int initialOrientation)
        {
            return initialOrientation switch
            {
                0 => positions[0] || positions[1] || positions[2],
                1 => positions[5] || positions[6] || positions[7],
                2 => positions[0] || positions[3] || positions[5],
                3 => positions[2] || positions[4] || positions[7] 
            };
        }

        public void Move(IEnumerable<Position> consideredPositions)
        {
            if (consideredPositions.Count(p => p == ConsideredPosition) == 2) return;

            Position = ConsideredPosition;
        }

        public bool Satisfied() => Position == ConsideredPosition;
    }

    private record Position(int Row, int Column)
    {
        public override string ToString()
        {
            return $"{Row}-{Column}";
        }
    }

    private static IEnumerable<Elf> Parse(IReadOnlyList<string> lines)
    {
        for (var row = 0; row < lines.Count; row++)
        {
            var line = lines[row];
            for (var column = 0; column < lines[0].Length; column++)
            {
                if (line[column] == '#') yield return new Elf(row, column);
            }
        }
    }
}