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
        //var c = Parse();

        //var b = new Board(c);
        ////b.Print();

        //for (int i = 0; i < 2500; i++)
        //{
        //    if (b.AddSand())
        //    {
        //        //b.Print();
        //        return i;
        //    }

        //    //b.Print();
        //}

        ////b.Print();

        return -1;
    }

    public long Part2()
    {
        var c = Parse();
        var maxRows = c.SelectMany(x => x).Select(x => x.x).Max();
        var minColumns = c.SelectMany(x => x).Select(x => x.y).Min();
        var maxColumns = c.SelectMany(x => x).Select(x => x.y).Max();

        var list = new List<(int row, int column)>();
        list.Add((maxRows + 2, 1));
        list.Add((maxRows + 2, maxColumns + 400));

        c.Add(list);

        var b = new Board(c);
        //b.Print();

        for (int i = 0; i < 1214900; i++)
        {
            if (b.AddSand())
            {
                b.Print();
                return i;
            }

            //b.Print();
        }

        //b.Print();

        return -1;
    }

    private List<List<(int x, int y)>> Parse()
    {
        var result = new List<List<(int x, int y)>>();
        foreach (var line in _input)
        {
            var lineResult = new List<(int x, int y)>();
            var coors = line.Split(" -> ");
            foreach (var coor in coors)
            {
                var splitted = coor.Split(',');
                lineResult.Add((int.Parse(splitted[1]), int.Parse(splitted[0])));
            }

            result.Add(lineResult);
        }

        return result;
    }


    public class Board
    {
        private readonly char[,] _cells;

        public int MinColumns { get; }
        public int MaxColumns { get; }
        public int MaxRows { get; }

        public Board(List<List<(int row, int column)>> coord)
        {
            MaxRows = coord.SelectMany(x => x).Select(x => x.row).Max();
            MinColumns = coord.SelectMany(x => x).Select(x => x.column).Min();
            MaxColumns = coord.SelectMany(x => x).Select(x => x.column).Max();
            _cells = new char[MaxRows + 2, MaxColumns + 2];

            for (var row = 0; row <= MaxRows + 1; row++)
            {
                for (var column = MinColumns - 1; column <= MaxColumns + 1; column++)
                {
                    _cells[row, column] = '.';
                }
            }

            SetRocks(coord);
        }

        private void SetRocks(List<List<(int row, int column)>> lines)
        {
            foreach (var coord in lines) 
            { 
                for (int i = 0; i < coord.Count - 1; i++)
                {
                    var a = coord[i];
                    var b = coord[i + 1];

                    var minRow = Math.Min(a.row, b.row);
                    var maxRow = Math.Max(a.row, b.row);
                    var minColumn = Math.Min(a.column, b.column);
                    var maxColumn = Math.Max(a.column, b.column);

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
                var next = GetPossible(position);
                if (!next.Any())
                {
                    if (position.row == 0)
                    {
                        return true;
                    }

                    _cells[position.row, position.column] = 'o';
                    return false;
                }

                position = next[0];
            }

            return true;
        }

        private List<(int row, int column)> GetPossible((int row, int column) position) 
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


                for (var column = 470; column <= 590; column++)
                {
                    Console.Write(_cells[row, column]);
                }

                Console.WriteLine("");
            }
        }
    }
}