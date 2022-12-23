using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day22 : IChallenge
{
    private readonly string[] _input;
    private  Board _board;

    public Day22(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var path = Parse();

        for (var i = 0; i < path.Length; i++)
        {
            var instruction = path[i];
            char? next = i < path.Length - 1 ? path[i + 1] : null;
            if (instruction == 'L')
            {
                _board.ChangeFacing(-1);
            }
            else if (instruction == 'R')
            {
                _board.ChangeFacing(1);
            }
            else
            {
                var move = int.Parse(instruction.ToString());
                if (next.HasValue && next.Value != 'R' && next.Value != 'L')
                {
                    move = int.Parse((instruction.ToString() + next.Value.ToString()).ToString());
                    i++;
                }

                _board.Move(move);
            } 

            //
        }
        //_board.Print();

        var result = (_board.Position.row + 1) * 1000 + (_board.Position.column + 1) * 4 + _board.Facing;
        return result;
    }

    public long Part2()
    {
        return -1;
    }

    private string Parse()
    {
        var boardLines = new List<string>();
        foreach (var line in _input)
        {
            if (string.IsNullOrWhiteSpace(line)) break;
            boardLines.Add(line);
        }

        _board = new Board(boardLines);

        return _input.Last();
    }

    private class Board
    {
        private readonly int[,] _rows;
        private readonly int _maxColumns;
        private readonly int _maxRows;

        public (int row, int column) Position { get; private set; }

        public int Facing { get; private set; }

        public Board(List<string> lines)
        {
            Position = (0, 0);
            _maxRows = lines.Count;
            _maxColumns = lines[0].Length;
            _rows = new int[_maxRows, _maxColumns];

            for (var row = 0; row < lines.Count; row++)
            {
                var line = lines[row];
                for (var column = 0; column < line.Length; column++)
                {
                    var value = line[column];
                    if (value == ' ')
                    {
                        _rows[row, column] = 0;
                    }
                    else if (value == '.')
                    {
                        _rows[row, column] = 1;
                    }
                    else
                    {
                        _rows[row, column] = 2;
                    }
                }
            }

            for (int column = 0; column < _maxColumns; column++)
            {
                if (_rows[0, column] == 1)
                {
                    Position = (0, column);
                    break;
                }
            }

            Facing = 0;
        }

        public void ChangeFacing(int delta)
        {
            Facing += delta;
            if (Facing < 0) Facing = 3;
            if (Facing > 3) Facing = 0;
        }

        public void Move(int move)
        {
            var newPosition = Position;
            var rowDelta = 0;
            var columnDelta = 0;

            if (Facing == 0)
            {
                columnDelta += move;
            }
            else if (Facing == 1)
            {
                rowDelta += move;
            }
            else if (Facing == 2)
            {
                columnDelta += move;
            }
            else
            {
                rowDelta += move;
            }

            while (rowDelta > 0)
            {
                var possiblePosition = FindNext(newPosition);
                if (_rows[possiblePosition.row, possiblePosition.column] == 2)
                {
                    break;
                }

                newPosition = possiblePosition;
                rowDelta--;
            }

            while (columnDelta > 0)
            {
                var possiblePosition = FindNext(newPosition);
                if (_rows[possiblePosition.row, possiblePosition.column] == 2)
                {
                    break;
                }

                newPosition = possiblePosition;
                columnDelta--;
            }

            Position = newPosition;
        }

        private (int row, int column) FindNext((int row, int column) position)
        {
            if (Facing == 0)
            {
                do
                {
                    position.column++;
                    if (IsOutside(position))
                    {
                        position.column = 0;
                    }

                } while (_rows[position.row, position.column] == 0);
            }
            else if (Facing == 1)
            {
                do
                {
                    position.row++;
                    if (IsOutside(position))
                    {
                        position.row = 0;
                    }

                } while (_rows[position.row, position.column] == 0);
            }
            else if (Facing == 2)
            {
                do
                {
                    position.column--;
                    if (IsOutside(position))
                    {
                        position.column = _maxColumns - 1;
                    }

                } while (_rows[position.row, position.column] == 0);
            }
            else
            {
                do
                {
                    position.row--;
                    if (IsOutside(position))
                    {
                        position.row = _maxRows - 1;
                    }

                } while (_rows[position.row, position.column] == 0);
            }

            //if (position.row > _maxRows)
            //{
            //    position.row = 0;
            //}

            //if (position.row < 0)
            //{
            //    position.row = _maxRows;
            //}

            //if (position.column > _maxColumns)
            //{
            //    position.column = 0;
            //}

            //if (position.column < 0)
            //{
            //    position.column = _maxColumns;
            //}

            return position;
        }

        private bool IsOutside((int row, int column) position)
        {
            if (position.row >= _maxRows)
            {
                return true;
            }

            if (position.row < 0)
            {
                return true;
            }

            if (position.column >= _maxColumns)
            {
                return true;
            }

            if (position.column < 0)
            {
                return true;
            }

            return false;
        }

        public void Print()
        {
            Console.WriteLine("");

            for (var row = 0; row < _maxRows; row++)
            {
                for (var column = 0; column < _maxColumns; column++)
                {
                    if (Position.row == row && Position.column == column)
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        if (_rows[row, column] == 0) Console.Write(" ");
                        if (_rows[row, column] == 1) Console.Write(".");
                        if (_rows[row, column] == 2) Console.Write("#");
                    }
                }

                Console.WriteLine("");
            }
        }
    }
}