using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

internal class Day11 : IChallenge
{
    private readonly string[] _input;

    public Day11(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        //var monkeys = new List<Monkey>
        //{
        //    new Monkey(new List<int> { 79, 98 }, worry => worry * 19, worry => worry % 23 == 0 ? 2 : 3),
        //    new Monkey(new List<int> { 54, 65, 75, 74 }, worry => worry + 6, worry => worry % 19 == 0 ? 2 : 0),
        //    new Monkey(new List<int> { 79, 60, 97 }, worry => worry * worry, worry => worry % 13 == 0 ? 1 : 3),
        //    new Monkey(new List<int> { 74 }, worry => worry + 3, worry => worry % 17 == 0 ? 0 : 1)
        //};
        var monkeys = new List<Monkey>
        {
            new Monkey(new List<int> { 78, 53, 89, 51, 52, 59, 58, 85 }, worry => worry * 3, worry => worry % 5 == 0 ? 2 : 7),
            new Monkey(new List<int> { 64 }, worry => worry + 7, worry => worry % 2 == 0 ? 3 : 6),
            new Monkey(new List<int> { 71, 93, 65, 82 }, worry => worry + 5, worry => worry % 13 == 0 ? 5 : 4),
            new Monkey(new List<int> { 67, 73, 95, 75, 56, 74 }, worry => worry + 8, worry => worry % 19 == 0 ? 6 : 0),
            new Monkey(new List<int> { 85, 91, 90 }, worry => worry + 4, worry => worry % 11 == 0 ? 3 : 1),
            new Monkey(new List<int> { 67, 96, 69, 55, 70, 83, 62 }, worry => worry * 2, worry => worry % 3 == 0 ? 4 : 1),
            new Monkey(new List<int> { 53, 86, 98, 70, 64 }, worry => worry + 6, worry => worry % 7 == 0 ? 7 : 0),
            new Monkey(new List<int> { 88, 64 }, worry => worry * worry, worry => worry % 17 == 0 ? 2 : 5),
        };

        var result = new Dictionary<int, long>();
        for (var index = 0; index < monkeys.Count; index++)
        {
            result.Add(index, 0);
        }

        for (var round = 0; round < 20; round++)
        {
            for (var index = 0; index < monkeys.Count; index++)
            {
                var monkey = monkeys[index];
                while (monkey.Items.Count > 0)
                {
                    var item = monkey.Items.Dequeue();
                    result[index]++;

                    item = monkey.Operation(item);
                    item = (int)Math.Floor((double)item / 3);
                    var newMonkey = monkey.Test(item);
                    monkeys[newMonkey].Items.Enqueue(item);
                }
            }

        }

        return result.Values
            .OrderDescending()
            .Take(2)
            .Aggregate((a, b) => a * b);
    }

    public long Part2()
    {
        //var monkeys = new List<Monkey>
        //{
        //    new Monkey(new List<int> { 79, 98 }, worry => worry * 19, worry => worry % 23 == 0 ? 2 : 3, 23),
        //    new Monkey(new List<int> { 54, 65, 75, 74 }, worry => worry + 6, worry => worry % 19 == 0 ? 2 : 0, 19),
        //    new Monkey(new List<int> { 79, 60, 97 }, worry => worry * worry, worry => worry % 13 == 0 ? 1 : 3, 13),
        //    new Monkey(new List<int> { 74 }, worry => worry + 3, worry => worry % 17 == 0 ? 0 : 1, 17)
        //};
        var monkeys = new List<Monkey>
        {
            new Monkey(new List<int> { 78, 53, 89, 51, 52, 59, 58, 85 }, worry => worry * 3, worry => worry % 5 == 0 ? 2 : 7, 5),
            new Monkey(new List<int> { 64 }, worry => worry + 7, worry => worry % 2 == 0 ? 3 : 6, 2),
            new Monkey(new List<int> { 71, 93, 65, 82 }, worry => worry + 5, worry => worry % 13 == 0 ? 5 : 4, 13),
            new Monkey(new List<int> { 67, 73, 95, 75, 56, 74 }, worry => worry + 8, worry => worry % 19 == 0 ? 6 : 0, 19),
            new Monkey(new List<int> { 85, 91, 90 }, worry => worry + 4, worry => worry % 11 == 0 ? 3 : 1, 11),
            new Monkey(new List<int> { 67, 96, 69, 55, 70, 83, 62 }, worry => worry * 2, worry => worry % 3 == 0 ? 4 : 1, 3),
            new Monkey(new List<int> { 53, 86, 98, 70, 64 }, worry => worry + 6, worry => worry % 7 == 0 ? 7 : 0, 7),
            new Monkey(new List<int> { 88, 64 }, worry => worry * worry, worry => worry % 17 == 0 ? 2 : 5, 17),
        };

        var common = monkeys.Select(m => m.Divider).Aggregate((a, b) => a * b);

        var result = new Dictionary<int, long>();
        for (var index = 0; index < monkeys.Count; index++)
        {
            result.Add(index, 0);
        }

        for (var round = 0; round < 10000; round++)
        {
            for (var index = 0; index < monkeys.Count; index++)
            {
                var monkey = monkeys[index];
                while (monkey.Items.Count > 0)
                {
                    var item = monkey.Items.Dequeue();
                    result[index]++;

                    item = monkey.Operation(item);
                    if (item > common)
                    {
                        item %= common;
                    }

                    var newMonkey = monkey.Test(item);
                    monkeys[newMonkey].Items.Enqueue(item);
                }
            }

            if (round == 1000 - 1 || round == 0)
            {
                Console.WriteLine($"Monkey 0 inspected items {result[0]} times.");
                Console.WriteLine($"Monkey 1 inspected items {result[1]} times.");
                Console.WriteLine($"Monkey 2 inspected items {result[2]} times.");
                Console.WriteLine($"Monkey 3 inspected items {result[3]} times.");
            }
        }

        return result.Values
            .OrderDescending()
            .Take(2)
            .Aggregate((a, b) => a * b);
    }

    private class Monkey
    {
        public Queue<long> Items { get; }
        public Func<long, long> Operation { get; }
        public Func<long, int> Test { get; }
        public int Divider { get; }

        public Monkey(IEnumerable<int> items, Func<long, long> operation, Func<long, int> test, int divider = 1)
        {
            Items = new Queue<long>();
            Operation = operation;
            Test = test;
            Divider = divider;

            foreach (var item in items)
            {
                Items.Enqueue(item);
            }
        }
    }
}