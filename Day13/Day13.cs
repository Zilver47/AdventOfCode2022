using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day13 : IChallenge
{
    private readonly string[] _input;

    public Day13(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var result = new List<int>();
        var pairs = Parse();
        var pairIndex = 1;
        for (var i = 0; i < pairs.Count - 1; i += 2)
        {
            var a = pairs[i];
            var b = pairs[i + 1];

            if (a.Compare(b) == -1) result.Add(pairIndex);

            pairIndex++;
        }

        return result.Sum();
    }

    public long Part2()
    {
        var pairs = Parse();
        var firstDivider = new Packet("[[2]]");
        pairs.Add(firstDivider);
        var secondDivider = new Packet("[[6]]");
        pairs.Add(secondDivider);

        pairs.Sort((a, b) => a.Compare(b));
        
        var indexOfFirstDivider = -1;
        var indexOfSecondDivider = -1;
        for (var i = 0; i <= pairs.Count; i++)
        {
            var row = pairs[i];
            if (row.Packets is [Packet { Packets: [ValuePacket { Value: 2 }] }])
            {
                indexOfFirstDivider = i + 1;
            }

            if (row.Packets is [Packet { Packets: [ValuePacket { Value: 6 }] }])
            {
                indexOfSecondDivider = i + 1;
            }
        }
        
        return indexOfFirstDivider * indexOfSecondDivider;
    }

    private List<Packet> Parse()
    {
        return (from line in _input where !string.IsNullOrWhiteSpace(line) select new Packet(line)).ToList();
    }

    private interface IPacket
    {
        public int Compare(IPacket other);
    }
    
    private class Packet : IPacket
    {
        public Packet(IPacket packet)
        {
            Packets = new List<IPacket> { packet };
        }

        public Packet(string line)
        {
            Packets = new List<IPacket>();

            line = line.Substring(1, line.Length - 2);

            var currentIndex = 0;
            while (line.Length > 0 && currentIndex < line.Length)
            {
                var element = line[currentIndex];
                if (element == '[')
                {
                    var endIndex = FindMatchingClosingElement(line[currentIndex..]) + currentIndex;
                    var packetLine = line.Substring(currentIndex, endIndex - currentIndex + 1);
                    Packets.Add(new Packet(packetLine));

                    line = line.Remove(currentIndex, endIndex - currentIndex + 1);
                }
                else if (IsNumber(element))
                {
                    // Take into account integers larger than 9
                    if (currentIndex != line.Length - 1 && IsNumber(line[currentIndex + 1]))
                    {
                        var number = string.Concat(element, line[currentIndex + 1]);
                        Packets.Add(new ValuePacket(int.Parse(number)));

                        currentIndex++;
                    }
                    else
                    {
                        Packets.Add(new ValuePacket(element - 48));
                    }

                    currentIndex++;
                }
                else
                {
                    currentIndex++;
                }
            }
        }

        private bool IsNumber(char value) => value >= 48 && value <= 57;

        private int FindMatchingClosingElement(string line)
        {
            var opening = 0;
            for (var i = 1; i < line.Length; i++)
            {
                var element = line[i];
                if (element == '[')
                {
                    opening++;
                }
                else if (element == ']')
                {
                    if (opening == 0)
                    {
                        return i;
                    }

                    opening--;
                }
            }

            return 0;
        }

        public List<IPacket> Packets { get; }
        
        public int Compare(IPacket other)
        {
            if (other is ValuePacket otherAsValuePacket)
            {
                other = new Packet(otherAsValuePacket);
            }

            if (other is Packet otherAsPacket)
            {
                var count = Math.Min(Packets.Count, otherAsPacket.Packets.Count);
                for (var i = 0; i < count; i++)
                {
                    var compareResult = Packets[i].Compare(otherAsPacket.Packets[i]);
                    if (compareResult == 1)
                    {
                        return 1;
                    }

                    if (compareResult == -1)
                    {
                        return -1;
                    }
                }

                if (Packets.Count == otherAsPacket.Packets.Count) return 0;
                return Packets.Count < otherAsPacket.Packets.Count ? -1 : 1;
            }

            return 0;
        }
    }

    private class ValuePacket : IPacket
    {
        public ValuePacket(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public int Compare(IPacket other)
        {
            if (other is ValuePacket otherAsValuePacket)
            {
                if (Value == otherAsValuePacket.Value) return 0;
                if (Value < otherAsValuePacket.Value) return -1;
                return 1;
            }

            var thisAsPacket = new Packet(this);
            return thisAsPacket.Compare(other);
        }
    }
}