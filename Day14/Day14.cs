using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day14 : IChallenge
{
    private readonly string[] _input;

    public Day14(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var coordinates = Parse();

        var board = new Board(coordinates);
        board.Print();

        for (var i = 0; i < 2500; i++)
        {
            if (board.AddSand())
            {
                board.Print();
                return i;
            }
        }

        return -1;
    }

    public long Part2()
    {
        var coordinates = Parse();
        var maxRows = coordinates.SelectMany(x => x).Select(x => x.x).Max();
        var maxColumns = coordinates.SelectMany(x => x).Select(x => x.y).Max();

        var listOfCoordinates = new List<(int row, int column)>
        {
            (maxRows + 2, 1),
            (maxRows + 2, maxColumns + 400)
        };
        coordinates.Add(listOfCoordinates);

        var board = new Board(coordinates);
        //board.Print();

        for (var i = 0; i < 100000; i++)
        {
            if (board.AddSand())
            {
                //board.Print();
                return i + 1;
            }
        }

        return -1;
    }

    private List<List<(int x, int y)>> Parse()
    {
        var result = new List<List<(int x, int y)>>();
        foreach (var line in _input)
        {
            var listOfCoordinates = new List<(int x, int y)>();
            var coordinates = line.Split(" -> ");
            foreach (var coordinate in coordinates)
            {
                var splitted = coordinate.Split(',');
                listOfCoordinates.Add((int.Parse(splitted[1]), int.Parse(splitted[0])));
            }

            result.Add(listOfCoordinates);
        }

        return result;
    }


    public class Board
    {
        private readonly char[,] _cells;

        public int MinColumns { get; }
        public int MaxColumns { get; }
        public int MaxRows { get; }

        public Board(List<List<(int row, int column)>> coordinates)
        {
            MaxRows = coordinates.SelectMany(x => x).Select(x => x.row).Max();
            MinColumns = coordinates.SelectMany(x => x).Select(x => x.column).Min();
            MaxColumns = coordinates.SelectMany(x => x).Select(x => x.column).Max();
            _cells = new char[MaxRows + 2, MaxColumns + 2];

            for (var row = 0; row <= MaxRows + 1; row++)
            {
                for (var column = MinColumns - 1; column <= MaxColumns + 1; column++)
                {
                    _cells[row, column] = '.';
                }
            }

            SetRocks(coordinates);
        }

        private void SetRocks(List<List<(int row, int column)>> allRocks)
        {
            foreach (var listOfCoordinates in allRocks) 
            { 
                for (var i = 0; i < listOfCoordinates.Count - 1; i++)
                {
                    var start = listOfCoordinates[i];
                    var end = listOfCoordinates[i + 1];

                    var minRow = Math.Min(start.row, end.row);
                    var maxRow = Math.Max(start.row, end.row);
                    var minColumn = Math.Min(start.column, end.column);
                    var maxColumn = Math.Max(start.column, end.column);

                    for (var row = minRow; row <= maxRow; row++)
                    {
                        for (var column = minColumn; column <= maxColumn; column++)
                        {
                            _cells[row, column] = '#';
                        }
                    }
                }
            }
        }

        public bool AddSand()
        {
            (int row, int column) position = (0, 500);

            while (position.row < MaxRows && position.column >= MinColumns && position.column <= MaxColumns)
            {
                var nextPositions = GetNextPositions(position);
                if (!nextPositions.Any())
                {
                    if (position.row == 0)
                    {
                        return true;
                    }

                    _cells[position.row, position.column] = 'o';
                    return false;
                }

                position = nextPositions[0];
            }

            return true;
        }

        private List<(int row, int column)> GetNextPositions((int row, int column) position) 
        {
            var possiblePositions = new List<(int row, int column)>
            {
                new() { row = position.row + 1, column = position.column },
                new() { row = position.row + 1, column = position.column - 1},
                new() { row = position.row + 1, column = position.column + 1}
            };

            return possiblePositions
                .Where(p => _cells[p.row, p.column] == '.')
                .ToList();
        }

        public void Print()
        {
            Console.WriteLine("");
            Console.WriteLine($"  {MinColumns}  {MaxColumns}");


            for (var row = 0; row <= MaxRows; row++)
            {
                Console.Write(row.ToString());
                Console.Write(" ");


                for (var column = MinColumns; column <= MaxColumns; column++)
                {
                    Console.Write(_cells[row, column]);
                }

                Console.WriteLine("");
            }
        }
    }
}