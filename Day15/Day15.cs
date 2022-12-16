using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day15 : IChallenge
{
    private readonly string[] _input;

    public Day15(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        //int row = 11;
        var row = 2000000;
        var sensors = _input.Select(l => new Sensor(l)).ToList();
        var minXSensors = sensors.Min(x => x.X);
        var minXBeacon = sensors.Min(x => x.Beacon.x);
        var minX = Math.Min(minXBeacon, minXSensors);

        var unavailable = sensors.Select(s => s.Intersect(row))
            .Where(r => r != null)
            .OrderBy(pair => pair.Start)
            .ThenBy(pair => pair.End)
            .ToList();

        var result = 0;
        var index = minX;
        foreach (var unavailablePositions in unavailable)
        {
            if (index < unavailablePositions.Start - 1)
            {
                index = unavailablePositions.Start;
            }

            if (index > unavailablePositions.End)
            {
                continue;
            }

            result += unavailablePositions.End - index;
            index = unavailablePositions.End;
        }


        return result;
    }

    public long Part2()
    {
        var max = 4000000;

        var sensors = _input.Select(l => new Sensor(l)).ToList();

        long x = -1;
        long y = -1;
        var previousUnavailable = -1;
        for (var row = 0; row <= max; row++)
        {
            var result = GetNumberOfUnavailableAndX(sensors, row, max);
            if (previousUnavailable != -1 && result.unavailable != previousUnavailable)
            {
                x = result.x;
                y = row;
                break;
            }

            previousUnavailable = result.unavailable;
        }


        return x * max + y;
    }
    private static (int x, int unavailable) GetNumberOfUnavailableAndX(List<Sensor> sensors, int row, int max)
    {
        var unavailable = sensors
            .Select(s => s.IntersectMax(row, max))
            .Where(r => r != null)
            .OrderBy(pair => pair.Start)
            .ThenBy(pair => pair.End)
            .ToList();

        (int x, int unavailable) result = new(0, 0);
        var index = 0;
        foreach (var unavailablePositions in unavailable)
        {
            if (index < unavailablePositions.Start - 1)
            {
                result.x = index + 1;
                index = unavailablePositions.Start;
            }

            if (index > unavailablePositions.End)
            {
                continue;
            }

            result.unavailable += unavailablePositions.End - index;
            index = unavailablePositions.End;
        }

        //for (int i = 0; i < 21; i++)
        //{
        //    if (intersects.Any(pair => i >= pair.Start && i <= pair.End))
        //    {
        //        Console.Write("#");
        //    }
        //    else
        //    {
        //        Console.Write(".");
        //    }
        //}

        return result;
    }

    private class Sensor
    {
        public int X { get; }
        public int Y { get; }

        public int Range { get; }

        public (int x, int y) Beacon { get; }

        public Sensor(string line)
        {
            var sensorAndBeacon = line.Split(':');
            var sensorCoordinates = sensorAndBeacon[0]
                .Replace("Sensor at x=", string.Empty)
                .Replace(" y=", string.Empty)
                .Split(',');
            X = int.Parse(sensorCoordinates[0]);
            Y = int.Parse(sensorCoordinates[1]);

            var beaconCoordinates = sensorAndBeacon[1]
                .Replace(" closest beacon is at x=", string.Empty)
                .Replace(" y=", string.Empty)
                .Split(',');
            Beacon = (int.Parse(beaconCoordinates[0]), int.Parse(beaconCoordinates[1]));

            var diffX = X > Beacon.x ? X - Beacon.x : Beacon.x - X;
            var diffY = Y > Beacon.y ? Y - Beacon.y : Beacon.y - Y;
            Range = diffX + diffY;
        }

        public Pair? Intersect(int row)
        {
            if (Y + Range < row) return null;
            if (Y - Range > row) return null;

            var diff = row > Y ? row - Y : Y - row;
            var start = X - Range + diff;
            var end = X + Range - diff;
            return new Pair(start, end);
        }

        public Pair? IntersectMax(int row, int max)
        {
            if (Y + Range < row) return null;
            if (Y - Range > row) return null;

            var diff = row > Y ? row - Y : Y - row;
            var start = X - Range + diff;
            var end = X + Range - diff;
            if (start < 0) start = 0;
            if (end > max) end = max;

            return new Pair(start, end);
        }
    }

    private record Pair(int Start, int End);
}