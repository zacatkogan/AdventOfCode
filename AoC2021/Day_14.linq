<Query Kind="Statements" />

checked
{
	var raw = File.ReadAllLines(MyExtensions.GetFilePathFromQueryDir("day14.txt"));

	var input = raw[0];
	var mappings = raw.Skip(2).Select(r => r.Split(' ')).ToDictionary(r => r[0], r => r[2]);

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

	var str = input;

	for (int j = 0; j < 10; j++)
	{
		str = Expand(str, mappings);//.Dump();
	}

	var result1 = str.GroupBy(q => q).OrderBy(q => q.Count()).ToList();
	(result1.Last().Count() - result1.First().Count()).Dump();

	var lastChar = input.Last();
	var tokenizedInput = Tokenize(input);

	for (int j = 0; j < 40; j++)
	{
		tokenizedInput = ExpandTokenize(tokenizedInput, mappings);
	}

	var result2 = tokenizedInput.Select(x => (x.Key[0], x.Value)).Append((lastChar, 1))
		.GroupBy(x => x.Item1)
		.Select(x => (x.Key, sum: x.Sum(y => y.Value)))
		.OrderBy(x => x.sum);
		
	(result2.Last().sum-result2.First().sum).Dump();
}