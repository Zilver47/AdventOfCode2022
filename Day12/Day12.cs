using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day12 : IChallenge
{
    private readonly string[] _input;

    public Day12(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var grid = new Board(_input.ToList());

        var start = new Tile
        {
            Row = grid.Start.row,
            Column = grid.Start.column
        };
        return grid.FindPath(start);
    }

    public long Part2()
    {
        var grid = new Board(_input.ToList());

        return grid.FindPath();
    }

    private class Board
    {
        private readonly int[,] _rows;
        private readonly int _maxColumns;
        private readonly int _maxRows;
        private bool[,] _visited;
        
        public (int row, int column) Start { get; }
        public (int row, int column) End { get; }

        public Board(List<string> lines)
        {
            _maxRows = lines.Count();
            _maxColumns = lines[0].Length;
            _rows = new int[_maxRows, _maxColumns];

            for (var row = 0; row < lines.Count; row++)
            {
                var line = lines[row];
                for (var column = 0; column < line.Length; column++)
                {
                    var value = line[column];
                    if (value == 'S')
                    {
                        Start = (row, column);
                        _rows[row, column] = 0;
                    }
                    else if (value == 'E')
                    {
                        End = (row, column);
                        _rows[row, column] = 27;
                    }
                    else
                    {
                        _rows[row, column] = value - 96;
                    }
                }
            }

            _visited = new bool[_maxRows, _maxColumns];
        }

        public long FindPath()
        {
            var allPaths = new List<long>();
            for (var i = 0; i < _rows.GetLength(0); i++)
            {
                for (var j = 0; j < _rows.GetLength(1); j++)
                {
                    if (_rows[i, j] == 1)
                    {
                        allPaths.Add(FindPath(new Tile() { Row = i, Column = j}));
                    }
                }
            }

            return allPaths.Where(x => x > 0).Min();
        }
        
        public long FindPath(Tile start)
        {
            _visited = new bool[_maxRows, _maxColumns];

            var finish = new Tile
            {
                Row = End.row,
                Column = End.column
            };

            var activeTiles = new List<Tile> { start };

            while (activeTiles.Any())
            {
                var lowestTotalSteps = activeTiles.Min(x => x.TotalSteps);
                var checkTiles = activeTiles.Where(x => x.TotalSteps == lowestTotalSteps).ToList();

                foreach (var checkTile in checkTiles)
                {
                    if (checkTile.Row == finish.Row && checkTile.Column == finish.Column)
                    {
                        return checkTile.TotalSteps;
                    }

                    _visited[checkTile.Row, checkTile.Column] = true;
                    activeTiles.Remove(checkTile);

                    var walkableTiles = GetWalkableTiles(checkTile);
                    foreach (var walkableTile in walkableTiles)
                    {
                        // We have already visited this tile so we don't need to do so again!
                        if (_visited[walkableTile.Row, walkableTile.Column])
                        {
                            continue;
                        }

                        // It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                        if (activeTiles.Any(x => x.Row == walkableTile.Row && x.Column == walkableTile.Column))
                        {
                            var existingTile = activeTiles.Single(x =>
                                x.Row == walkableTile.Row && x.Column == walkableTile.Column);
                            if (existingTile.TotalSteps > walkableTile.TotalSteps)
                            {
                                activeTiles.Remove(existingTile);
                                activeTiles.Add(walkableTile);
                            }
                        }
                        else
                        {
                            // We've never seen this tile before so add it to the list. 
                            activeTiles.Add(walkableTile);
                        }
                    }
                }
            }

            return -1;
        }

        private List<Tile> GetWalkableTiles(Tile current)
        {
            var possibleTiles = new List<Tile>
            {
                new() { Row = current.Row, Column = current.Column - 1, TotalSteps = current.TotalSteps },
                new() { Row = current.Row, Column = current.Column + 1, TotalSteps = current.TotalSteps},
                new() { Row = current.Row - 1, Column = current.Column, TotalSteps = current.TotalSteps },
                new() { Row = current.Row + 1, Column = current.Column, TotalSteps = current.TotalSteps },
            };

            possibleTiles = possibleTiles
                .Where(tile => tile.Row >= 0 && tile.Row < _maxRows)
                .Where(tile => tile.Column >= 0 && tile.Column < _maxColumns)
                .Where(tile => _rows[tile.Row, tile.Column] - _rows[current.Row, current.Column] <= 1)
                .ToList();
            
            foreach (var tile in possibleTiles)
            {
                tile.TotalSteps = current.TotalSteps + 1;
            }

            return possibleTiles;
        }
    }

    private record Tile
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int TotalSteps { get; set; }

        public override string ToString()
        {
            return $"{Row}-{Column} (Total steps: {TotalSteps};";
        }
    }
}