<Query Kind="Statements" />

var opens = new List<char> { '{', '(', '[', '<' };
var closes = new List<char> { '}', ')', ']', '>' };

var rawInput = File.ReadAllLines(MyExtensions.GetFilePathFromQueryDir("Day10.txt"));

var invalidCharScores = new Dictionary<char, int> { { ')', 3 }, { ']', 57 }, { '}', 1197 }, {'>', 25137 }};
var autoCompleteCharScores = new Dictionary<char, int> { { ')', 1 }, { ']', 2 }, { '}', 3 }, {'>', 4 }};

rawInput.Select(i => new { valid = IsValidLine(i, out char invalidChar), ch = invalidChar})
	.Where(x => !x.valid)
	.Select(x => x.ch)
	.GroupBy(x => x)
	.Select(x => new { key = x.Key, count = x.Count()})
	.Select(x => invalidCharScores[x.key] * x.count)
	.Sum()
	.Dump();

var scores = rawInput.Where(x => IsValidLine(x, out _))
	.Select(AutoCompleteLine)
	.OrderBy(x => x)
	.ToList()
	.Dump();
scores[(scores.Count / 2)].Dump();
scores[26].Dump();

// checks for an invalid input, returns true if valid, returns false and first invalid char if not.
bool IsValidLine(IEnumerable<char> input, out char invalidChar)
{
	var stack = new Stack<char>();

	foreach (var i in input)
	{
		if (opens.Contains(i))
		{
			stack.Push(i);
		}
		else
		{
			var opener = stack.Pop();
			var closer = i;

			if (opens.Contains(opener) && closer == closes[opens.IndexOf(opener)])
				continue;

			else
			{
				invalidChar = closer;
				return false;
			}
		}
	}
	
	invalidChar = '\0';
	return true;
}

// returns the Score of the autocompleted line
long AutoCompleteLine(IEnumerable<char> input)
{
	var stack = new Stack<char>();

	foreach (var i in input)
	{
		if (opens.Contains(i))
		{
			stack.Push(i);
		}
		else
		{
			stack.Pop();
		}
	}
	
	// need to pop the remaining Opens, and close them off - calculating the score
	long score = 0;
	foreach (var i in stack)
	{
		score *= 5;
		var closer = closes[opens.IndexOf(i)];
		score += autoCompleteCharScores[closer];
	}
	
	return score;
}
