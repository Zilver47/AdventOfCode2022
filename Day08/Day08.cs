using System;
using System.Collections.Generic;
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
        private readonly int[,] _rows;

        public int MaxColumns { get; }
        public int MaxRows { get; }

        public Board(List<string> line)
        {
            MaxRows = line.Count;
            MaxColumns = line[0].Length;
            _rows = new int[MaxRows, MaxColumns];

            for (var row = 0; row < MaxRows; row++)
            {
                for (var column = 0; column < MaxColumns; column++)
                {
                    _rows[row, column] = int.Parse(line[row].ToCharArray()[column].ToString());
                }
            }
        }

        public long HighestScenicScore()
        {
            long result = 0;
            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                for (var j = 0; j < _rows.GetLength(1); j++)
                {
                    var score = HighestScenicScore(i, j);
                    if (score > result)
                    {
                        result = score;
                    }
                }
            }

            return result;
        }

        private long HighestScenicScore(int row, int column)
        {
            var height = _rows[row, column];
            
            return CountFromTop(row, column, height)
                   * CountFromBottom(row, column, height)
                   * CountFromLeft(row, column, height)
                   * CountFromRight(row, column, height);
        }

        private long CountFromRight(int row, int column, int height)
        {
            long result = 0;
            for (var i = column + 1; i < MaxColumns; i++)
            {
                result++;
                if (_rows[row, i] >= height)
                {
                    return result;
                }
            }
            return result;
        }

        private long CountFromLeft(int row, int column, int height)
        {
            long result = 0;
            for (var i = column - 1; i >= 0; i--)
            {
                result++;
                if (_rows[row, i] >= height)
                {
                    return result;
                }
            }
            return result;
        }

        private long CountFromBottom(int row, int column, int height)
        {
            long result = 0;
            for (var i = row + 1; i < MaxRows; i++)
            {
                result++;
                if (_rows[i, column] >= height)
                {
                    return result;
                }
            }
            return result;
        }

        private long CountFromTop(int row, int column, int height)
        {
            long result = 0;
            for (var i = row - 1; i >= 0; i--)
            {
                result++;
                if (_rows[i, column] >= height)
                {
                    return result;
                }
            }
            return result;
        }

        public int NumberOfVisibleTrees()
        {
            var result = 0;
            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                for (var j = 0; j < _rows.GetLength(1); j++)
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

            var height = _rows[row, column];

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
                if (_rows[row, i] >= height)
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
                if (_rows[row, i] >= height)
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
                if (_rows[i, column] >= height)
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
                if (_rows[i, column] >= height)
                {
                    return false;
                }
            }
            return true;
        }
    }
}