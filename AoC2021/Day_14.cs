using System.Linq;
using System.Text;

namespace AdventOfCode.AoC2021
{

    public class Day_14 : BaseDay

    {
        public override object Solve1()
        {
            checked
            {
                var input = DataLines[0];
                var mappings = DataLines.Skip(2).Select(r => r.Split(' ')).ToDictionary(r => r[0], r => r[2]);

                var str = input;

                for (int j = 0; j < 10; j++)
                {
                    str = Expand(str, mappings);//.Dump();
                }

                var result1 = str.GroupBy(q => q).OrderBy(q => q.Count()).ToList();
                return (result1.Last().Count() - result1.First().Count());
            }
        }

        public override object Solve2()
        {
            var lastChar = DataLines[0].Last();
            var tokenizedInput = Tokenize(DataLines[0]);
            var mappings = DataLines.Skip(2).Select(r => r.Split(' ')).ToDictionary(r => r[0], r => r[2]);

            for (int j = 0; j < 40; j++)
            {
                tokenizedInput = ExpandTokenize(tokenizedInput, mappings);
            }

            var result2 = tokenizedInput.Select(x => (x.Key[0], x.Value))
                .Append((lastChar, 1)).ToList()
                .GroupBy(x => x.Item1)
                .Select(x => (x.Key, sum: x.Sum(y => y.Value)))
                .OrderBy(x => x.sum);

            return result2.Last().sum - result2.First().sum;
        }

        string Expand(string source, Dictionary<string, string> mappings)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < source.Length - 1; i++)
            {
                sb.Append(source[i]);

                var key = source.Substring(i, 2);
                sb.Append(mappings[key]);
            }

            sb.Append(source.Last());

            return sb.ToString();
        }

        Dictionary<string, decimal> Tokenize(string source)
        {
            return Enumerable.Range(0, source.Length - 1)
                .Select(s => source.Substring(s, 2))
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => (decimal)x.Count());
        }

        Dictionary<string, decimal> ExpandTokenize(Dictionary<string, decimal> input, Dictionary<string, string> mapping)
        {
            Dictionary<string, decimal> output = new();
            foreach (var entry in input)
            {
                var newElement = mapping[entry.Key];

                AddOrUpdate(entry.Key[0] + newElement, entry.Value);
                AddOrUpdate(newElement + entry.Key[1], entry.Value);

                void AddOrUpdate(string key, decimal value)
                {
                    if (output.ContainsKey(key))
                        output[key] += value;
                    else
                        output.Add(key, value);
                }
            }

            return output;
        }
    }
}
