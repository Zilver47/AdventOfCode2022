using System.Diagnostics;

var timer = new Stopwatch();
timer.Start();

var lines = File.ReadAllLines($"{nameof(Day02)}\\input.txt");
var generator = new Day02(lines);

Console.WriteLine($"--- {generator.GetType().Name} ---");
Console.Write("Answer 1: ");
WriteAnswer(generator.Part1().ToString());
Console.WriteLine();

timer.Stop();
Console.WriteLine($"Verstreken tijd: {timer.Elapsed.TotalSeconds}");

timer.Restart();

Console.WriteLine();
Console.Write("Answer 2: ");
WriteAnswer(generator.Part2().ToString());
Console.WriteLine();

timer.Stop();
Console.WriteLine($"Verstreken tijd: {timer.Elapsed.TotalSeconds}");

Console.ReadLine();

static void WriteAnswer(string value)
{
    var originalForegroundColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write(value);
    Console.ForegroundColor = originalForegroundColor;
}