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

        var directorySizes = system.GetAllDirectoriesSizes();
        return directorySizes
            .Where(size => size <= 100000)
            .Sum();
    }

    public long Part2()
    {
        var system = BuildSystem();
        
        var directorySizes = system.GetAllDirectoriesSizes();

        var available = 70000000 - system.Size();
        var needed = 30000000 - available;
        return directorySizes
            .Where(size  => size >= needed)
            .Min();
    }

    private Directory BuildSystem()
    {
        var system = new Directory("/", null);
        var current = system;
        foreach (var command in _input)
        {
            var splitted = command.Split(' ');
            if (command.StartsWith("$ cd"))
            {
                if (command.EndsWith("/"))
                {
                    // Do nothing on purpose
                }
                else if (command.EndsWith(".."))
                {
                    current = current.Parent;
                }
                else
                {
                    var name = splitted[2];
                    current = current.Children.First(c => c.Name == name) as Directory;
                }
            }
            else if (command.StartsWith("$ ls"))
            {
                // Do nothing on purpose
            }
            else if (command.StartsWith("dir "))
            {
                current.AddChild(new Directory(splitted[1], current));
            }
            else
            {
                current.AddChild(new File(splitted[1], int.Parse(splitted[0]), current));
            }
        }

        return system;
    }

    abstract class FileSystemNode
    {
        public string Name { get; }

        public Directory Parent { get; }

        protected FileSystemNode(string name, Directory parent)
        {
            Name = name;
            Parent = parent;
        }

        public abstract long Size();

        public abstract void Print(int depth);
    }

    class Directory : FileSystemNode
    {
        public List<FileSystemNode> Children { get; }

        public Directory(string name, Directory parent) : base(name, parent)
        {
            Children = new List<FileSystemNode>();
        }

        public void AddChild(FileSystemNode data)
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

        public IEnumerable<long> GetAllDirectoriesSizes()
        {
            var result = new List<long>();
            foreach (var childDirectories in Children.OfType<Directory>().Select(child => child.GetAllDirectoriesSizes()))
            {
                result.AddRange(childDirectories);
            }

            result.Add(Size());

            return result;
        }
    }

    private class File : FileSystemNode
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