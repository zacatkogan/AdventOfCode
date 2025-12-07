namespace AdventOfCode.AoC2025;

public class Day_06 : BaseDay
{
    public override object Solve1()
    {
        var data = DataLines.Select(x => x.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)).ToList();

        var total = 0L;

        for (int i = 0; i < data[0].Length; i++)
        {
            var a = long.Parse(data[0][i]);
            var b = long.Parse(data[1][i]);
            var c = long.Parse(data[2][i]);
            var d = long.Parse(data[3][i]);

            var operation = data[4][i];
            if (operation == "*")
            {
                total += (a * b * c * d);
            }
            else if (operation == "+")
            {
                total += (a + b + c + d);
            }
        }

        return total;
    }

    public override object Solve2()
    {
        var data = DataLines;

        var total = 0L;

        var isNewOperation = true;
        char operation = '+';
        long opTotal = 0;
        for (int i = 0; i < data[0].Length; i++)
        {
            var a = data[0][i].ToString();
            var b = data[1][i].ToString();
            var c = data[2][i].ToString();
            var d = data[3][i].ToString();

            var l = new List<string> { a, b, c, d };

            if (l.All(string.IsNullOrWhiteSpace))
            {
                isNewOperation = true;
                total += opTotal;
                opTotal = 0;
                continue;
            }

            if (isNewOperation)
            {
                isNewOperation = false;
                operation = data[4][i];

                if (operation == '*')
                {
                    opTotal = 1;
                }
            }

            var num = 0L;
            var str = a + b + c + d;
            try
            {
                num = long.Parse(str);
            }
            catch
            {
            }

            if (operation == '*')
            {
                opTotal *= num;
            }
            else if (operation == '+')
            {
                opTotal += num;
            }

        }

        total += opTotal;

        return total;
    }
}
