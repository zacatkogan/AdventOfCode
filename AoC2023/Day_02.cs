namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;
    using MathNet.Numerics.Optimization;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class Day_02: BaseDay
    {
        public override object Solve1()
        {
            var games = DataLines.Select(ParseLine).ToList();
            return games.Where(x => x.results.All(x => x.Blue <= 14 && x.Green <= 13 && x.Red <= 12)).Sum(x => x.Id);
        }

        public override object Solve2()
        {
            var games = DataLines.Select(ParseLine).ToList();

            return games.Select(x => new
                {
                    red = x.results.Max(r => r.Red),
                    green = x.results.Max(r => r.Green),
                    blue = x.results.Max(r => r.Blue),
                })
                .Select(x => x.red * x.green * x.blue)
                .Sum();
        }

        public Game ParseLine(string s)
        {
            var g = new Game();

            g.Id = RegexUtils.GetInts(s.Split(":").First()).First();

            var resultsRaw = s.Split(":").Last();
            var resultRaw = resultsRaw.Split(";");

            foreach (var result in  resultRaw)
            {
                var r = new Result();

                var outcomes = result.Split(",");
                foreach (var outcome in outcomes)
                {
                    var i = RegexUtils.GetInts(outcome).First();
                    if (outcome.Contains("blue"))
                    {
                        r.Blue = i;
                    }
                    else if (outcome.Contains("red"))
                    {
                        r.Red = i;
                    }
                    else if (outcome.Contains("green"))
                    {
                        r.Green = i;
                    }
                }

                g.results.Add(r);
            }

            return g;
        }

        public class Game
        {
            public int Id;
            public List<Result> results = new List<Result>();
        }

        public class Result
        {
            public int Blue;
            public int Red;
            public int Green;
        }

        string TestData = "";
    }
}
