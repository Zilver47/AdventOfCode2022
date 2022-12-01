internal class Day01 : IChallenge
{
    private readonly string[] _input;

    public Day01(string[] input)
    {
        _input = input;
    }

    public long Part1()
    {
        var all = new List<long>();
        long elf = 0;
        foreach (var line in _input)
        {
            if (string.IsNullOrEmpty(line))
            {
                all.Add(elf);
                elf = 0;
            }
            else
            {
                elf += long.Parse(line);
            }
        }

        all.Add(elf);

        return all.Max();
    }

    public long Part2()
    {
        var all = new List<long>();
        long elf = 0;
        foreach (var line in _input)
        {
            if (string.IsNullOrEmpty(line))
            {
                all.Add(elf);
                elf = 0;
            }
            else
            {
                elf += long.Parse(line);
            }
        }

        all.Add(elf);

        return all.Order().TakeLast(3).Sum();
    }
}