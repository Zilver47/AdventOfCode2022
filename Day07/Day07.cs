using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Day07 : IChallenge
{
    private readonly string[] _input;

    public Day07(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var system = BuildSystem();

        //system.Print(0);

        var allDirectories = system.GetDirectories();

        return allDirectories
            .Where(d => d.Item2 <= 100000)
            .Sum(d => d.Item2);
    }

    public long Part2()
    {
        var system = BuildSystem();
        
        var allDirectories = system.GetDirectories();

        var available = 70000000 - system.Size();
        var needed = 30000000 - available;
        return allDirectories
            .Where(d => d.Item2 >= needed)
            .Min(d => d.Item2);
    }

    private Directory BuildSystem()
    {
        var system = new Directory("/", null);
        var current = system;
        foreach (var command in _input)
        {
            if (command.StartsWith("$ cd"))
            {
                if (command.EndsWith("/"))
                {
                }
                else if (command.EndsWith(".."))
                {
                    current = current.Parent;
                }
                else
                {
                    var name = command.Replace("$ cd ", string.Empty);
                    current = current.Children.First(c => c.Name == name) as Directory;
                }
            }
            else if (command.StartsWith("dir "))
            {
                var name = command.Replace("dir ", string.Empty);
                current.AddChild(new Directory(name, current));
            }
            else if (command.StartsWith("$ ls"))
            {
            }
            else
            {
                var splitted = command.Split(' ');
                current.AddChild(new File(splitted[1], int.Parse(splitted[0]), current));
            }
        }

        return system;
    }

    abstract class Base
    {
        public string Name { get; }

        public Directory Parent { get; }

        protected Base(string name, Directory parent)
        {
            Name = name;
            Parent = parent;
        }

        public abstract long Size();

        public abstract void Print(int depth);
    }

    class Directory : Base
    {
        public List<Base> Children { get; }

        public Directory(string name, Directory parent) : base(name, parent)
        {
            Children = new List<Base>();
        }

        public void AddChild(Base data)
        {
            Children.Add(data);
        }

        public override long Size()
        {
            return Children.Sum(child => child.Size());
        }

        public override void Print(int depth)
        {
            for (var i = 0; i < depth; i++)
            {
                Console.Write("  ");
            }

            Console.WriteLine($"- {Name} (dir)");

            foreach (var child in Children)
            {
                child.Print(depth + 1);
            }
        }

        public IEnumerable<Tuple<string, long>> GetDirectories()
        {
            var result = new List<Tuple<string, long>>();
            foreach (var childDirectories in Children.OfType<Directory>().Select(child => child.GetDirectories()))
            {
                result.AddRange(childDirectories);
            }

            result.Add(new Tuple<string, long>(Name, Size()));

            return result;
        }
    }

    private class File : Base

    {
        private readonly int _size;

        public File(string name, int size, Directory parent) : base(name, parent)
        {
            _size = size;
        }

        public override long Size()
        {
            return _size;
        }

        public override void Print(int depth)
        {
            for (var i = 0; i < depth; i++)
            {
                Console.Write("  ");
            }

            Console.WriteLine($"- {Name} (file, size={_size})");
        }
    }
}