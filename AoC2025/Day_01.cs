using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.AoC2025;

public class Day_01 : BaseDay
{
    public override object Solve1()
    {
        var pointer = 50;
        var password = 0;

        foreach (var line in DataLines)
        {
            var dir = line[..1];
            var num = int.Parse(line[1..]);

            if (dir == "L")
            {
                num = -num;
            }

            pointer = Modulo(pointer + num, 100);
            if (pointer == 0 || pointer == 100)
            {
                password++;
            }
        }

        return password;
    }

    public override object Solve2()
    {
        return Part2Naive();
    }

    public int Part2Naive()
    {
        var pointer = 50;
        var password = 0;

        foreach (var line in DataLines)
        {
            var step = 1;
            var num = int.Parse(line[1..]);

            if (line[..1] == "L")
            {
                step = -step;
            }

            for (int i  = 0; i < num; i++)
            {
                pointer += step;
                if (pointer == 100)
                {
                    pointer = 0;
                }
                if (pointer == -1)
                {
                    pointer = 99;
                }

                if (pointer == 0)
                {
                    password++;
                }
            }

            Console.WriteLine($"{line}: {pointer}, {password}");
        }

        return password;
    }

    public int Part2Simplified()
    {
        var pointer = 50;
        var password = 0;

        foreach (var line in DataLines)
        {
            int ticks = 0;

            var dir = line[..1];
            var num = int.Parse(line[1..]);

            if (dir == "L")
            {
                num = -num;
            }

            pointer += num;

            Console.WriteLine("{2} - Rotated {0}, final value: {1}", num, pointer, line);

            if (pointer == 0 && num != 0)
            {
                ticks++;
            }

            if (pointer < 0)
            {
                while (pointer < 0)
                {
                    pointer += 100;
                    ticks++;
                }
            }
            else if (pointer >= 100)
            {
                while (pointer >= 100)
                {
                    pointer -= 100;
                    ticks++;
                }
            }

            Console.WriteLine("{0} ticks", ticks);

            password += ticks;
        }

        return password;
    }


    public string testData = @"";

    public int Modulo(int value, int mod)
    {
        return (value + mod) % mod;
    }
}



// not 6707
//6689