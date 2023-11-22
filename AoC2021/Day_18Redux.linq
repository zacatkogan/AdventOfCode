<Query Kind="Program" />

string[] rawInput = File.ReadAllLines(MyExtensions.GetFilePathFromQueryDir("day18.txt"));
string[] rawInputTest = @"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]
[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]
[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]
[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]
[7,[5,[[3,8],[1,4]]]]
[[2,[2,2]],[8,[8,1]]]
[2,9]
[1,[[[9,3],9],[[9,0],[0,7]]]]
[[[5,[7,4]],7],1]
[[[[4,2],2],6],[8,7]]".Split("\r\n");


void Main()
{
//	GetMagnitude("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]").Dump();
	
	//var rawInput = rawInputTest;
	
	Part1();
	Part2();
}

public void Part1()
{
	var s = rawInput[0];
	for (int i = 1; i < rawInput.Length; i++)
	{
		s = Add(s, rawInput[i]);
		s = Simplify(s);
	}
	GetMagnitude(s).Dump("Part1");
}

public void Part2()
{
	var indexes = from i in Enumerable.Range(0, rawInput.Length)
				  from j in Enumerable.Range(0, rawInput.Length)
				  where i != j
				  select (i, j);
	
	var magnitudes = indexes
		.Select(index => (first: rawInput[index.i], second: rawInput[index.j]))		
		.Select(x => Add(x.first, x.second))
		.Select(Simplify)
		.Select(GetMagnitude)
		.Max()
		.Dump().Dump("Part 2");
	
}

public string Add(string input1, string input2)
{
	return $"[{input1},{input2}]";
}

public string Explode(string str, int startingIndex)
{
	// find the ending bracket - could be len 5..len 7 depending on if both are 2 digit nos.
	
	var endingBracket = str.IndexOf(']', startingIndex);
	var elementLength = endingBracket - startingIndex + 1;
	
	var start = str.Substring(0, startingIndex);//.Dump();
	var element = str.Substring(startingIndex, elementLength);//.Dump();
	var end = str.Substring(startingIndex+elementLength);//.Dump();

	var parts = element.Split(new[] { ",", "[", "]"}, StringSplitOptions.RemoveEmptyEntries);
	var left = int.Parse(parts[0]);
	var right = int.Parse(parts[1]);
	
	// start constructing the new string...
	// backtrack through `start` until we reach a number...
	for (int p = start.Length-1; p > 0; p--)
	{
		if (char.IsNumber(start[p]))
		{
			var temp = p;
			// is it a 2 digit number?
			if (char.IsNumber(start[p-1]))
				temp = p - 1;
			
			var s = start.Substring(0, temp);//.Dump();
			var e = start.Substring(p + 1);//.Dump();
			var val = int.Parse(start[temp..(p+1)]) + left;

			start = $"{s}{val.ToString()}{e}";//.Dump();
			break;
		}
	}

	for (int p = 0; p < end.Length; p++)
	{
		if (char.IsNumber(end[p]))
		{
			var temp = p;
			// is it a 2 digit number?
			if (char.IsNumber(end[p + 1]))
				temp = p + 1;
				
			var s = end.Substring(0, p);//.Dump();
			var e = end.Substring(temp + 1);//.Dump();
			var val = int.Parse(end[p..(temp+1)]) + right;

			end = $"{s}{val.ToString()}{e}";//.Dump();
			break;
		}
	}

	return $"{start}0{end}";//.Dump();
}

public string Split(string str, int startingIndex)
{
	string start = str.Substring(0, startingIndex);
	string val = str.Substring(startingIndex, 2);
	string end = str.Substring(startingIndex+2);
	
	var newValue = int.Parse(val) / 2.0;

	return $"{start}[{(int)(Math.Floor(newValue))},{(int)(Math.Ceiling(newValue))}]{end}";
}

public bool Simplify(string input, out string output)
{
	int level = -1;

	for (int pos = 0; pos < input.Length; pos++)
	{
		char c = input[pos];

		if (c == '[')
		{
			level++;
			if (level == 4)
			{
				output = Explode(input, pos);
				return true;
			}
		}
		else if (c == ']')
		{
			level--;
		}
		else continue;
	}

	for (int pos = 0; pos < input.Length; pos++)
	{
		char c = input[pos];

		if (char.IsNumber(c))
		{
			// check if the next char is also a number. if it is, we can split
			if (char.IsNumber(input[pos + 1]))
			{
				output = Split(input, pos);
				return true;
			}
		}
		else continue;
	}

	output = input;
	return false;
}

public string Simplify(string input)
{
	string prev = input;
	string current = input;

	while (Simplify(prev, out current))
	{
		//current.Dump();
		prev = current;
	}
		
	return current;
}

// somethign something recursive
public int GetMagnitude(string input)
{
	var re = new Regex("\\[([0-9]*,[0-9]*)\\]");

	while (true)
	{
		var match = re.Match(input);
		
		if (!match.Success)
			break;
		
		var capture = match.Groups.Values.Last().Value;
		var parts = capture.Split(',');
		var newValue = (3 * int.Parse(parts[0])) + (2 * int.Parse(parts[1]));

		input = input.Replace(match.Value, newValue.ToString());

	}
	
	return int.Parse(input);
}