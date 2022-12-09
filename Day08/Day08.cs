using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day08 : IChallenge
{
    private readonly string[] _input;

    public Day08(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var board = new Board(_input.ToList());

        return board.NumberOfVisibleTrees();
    }

    public long Part2()
    {
        var board = new Board(_input.ToList());
        return board.HighestScenicScore();
    }

    public class Board
    {
        private readonly int[,] _cells;
        private readonly int[][] _rows;
        private readonly int[][] _columns;

        public int MaxColumns { get; }
        public int MaxRows { get; }

        public Board(List<string> line)
        {
            MaxRows = line.Count;
            MaxColumns = line[0].Length;
            _cells = new int[MaxRows, MaxColumns];

            for (var row = 0; row < MaxRows; row++)
            {
                for (var column = 0; column < MaxColumns; column++)
                {
                    _cells[row, column] = int.Parse(line[row].ToCharArray()[column].ToString());
                }
            }

            _rows = GetRows();
            _columns = GetColumns();
        }


        public long HighestScenicScore()
        {
            long result = 0;
            for (var i = 0; i < _cells.GetLength(0); i++)
            {
                for (var j = 0; j < _cells.GetLength(1); j++)
                {
                    var score = GetScenicScore(i, j);
                    if (score > result)
                    {
                        result = score;
                    }
                }
            }

            return result;
        }

        private long GetScenicScore(int row, int column)
        {
            var height = _cells[row, column];

            return GetOneDirectionScore(height, _columns[column][..row].Reverse())
                   * GetOneDirectionScore(height, _columns[column][(row + 1)..])
                   * GetOneDirectionScore(height, _rows[row][..column].Reverse())
                   * GetOneDirectionScore(height, _rows[row][(column + 1)..]);
        }

        private int GetOneDirectionScore(int height, IEnumerable<int> trees)
        {
            var result = 0;
            foreach (var tree in trees)
            {
                result++;
                if (tree >= height)
                {
                    return result;
                }
            }
            return result;
        }

        public int NumberOfVisibleTrees()
        {
            var result = 0;
            for (var i = 0; i < _cells.GetLength(0); i++)
            {
                for (var j = 0; j < _cells.GetLength(1); j++)
                {
                    result += IsVisible(i, j) ? 1 : 0;
                }
            }

            return result;
        }

        private bool IsVisible(int row, int column)
        {
            if (row == 0 || row == MaxRows - 1 || column == 0 || column == MaxColumns - 1)
            {
                return true;
            }

            var height = _cells[row, column];

            if (!IsVisibleFromTop(row, column, height) &&
                !IsVisibleFromBottom(row, column, height) &&
                !IsVisibleFromLeft(row, column, height) &&
                !IsVisibleFromRight(row, column, height)) return false;

            return true;
        }

        private bool IsVisibleFromRight(int row, int column, int height)
        {
            for (var i = column + 1; i < MaxColumns; i++)
            {
                if (_cells[row, i] >= height)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsVisibleFromLeft(int row, int column, int height)
        {
            for (var i = 0; i < column; i++)
            {
                if (_cells[row, i] >= height)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsVisibleFromBottom(int row, int column, int height)
        {
            for (var i = row + 1; i < MaxRows; i++)
            {
                if (_cells[i, column] >= height)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsVisibleFromTop(int row, int column, int height)
        {
            for (var i = 0; i < row; i++)
            {
                if (_cells[i, column] >= height)
                {
                    return false;
                }
            }
            return true;
        }

        private int[][] GetRows()
        {
            return Enumerable.Range(0, MaxRows)
                .Select(row => Enumerable.Range(0, MaxColumns).Select(col => _cells[row, col]).ToArray()).ToArray();
        }

        private int[][] GetColumns()
        {
            return Enumerable.Range(0, MaxColumns)
                .Select(col => Enumerable.Range(0, MaxRows).Select(row => _cells[row, col]).ToArray()).ToArray();
        }
    }
}