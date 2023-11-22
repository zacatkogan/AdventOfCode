<Query Kind="Statements" />

var sourcePath = MyExtensions.GetFilePathFromQueryDir("day8.txt");

string[] source = File.ReadAllLines(sourcePath);

(string[] signals, string[] display) ReadLine(string input)
{
	var temp = input.Split(" ");
	var signals = temp.Take(10).ToArray();
	var output = temp.Skip(11).Take(4).ToArray();
	
	return (signals, output);
}

// part 1
source.Select(ReadLine).Select(x => x.display)
	.Sum(x => x.Count(y => y.Length == 2 || y.Length == 3 || y.Length == 4 || y.Length == 7)).Dump();
	
// part 2
source.Select(ReadLine)
	.Select(x => (signals: ParseSignals(x.signals), x.display))
	.Select(line => line.display
        .Select(dispDigit =>                                                // iterate over the display inputs
		    line.signals.IndexOf(                                           // return the index of the Signal
			    line.signals.First(z => AreEqual(z, dispDigit)))))          //     by matching on the display input
	.Select(i => i.Select(j => j.ToString()))								// convert indexes to strings
	.Select(i => string.Join("",i))											// then join into a single string-number
	.Select(int.Parse)														// convert back to digits
	.Sum() 																	// sum
	.Dump();

// all the magic happens here
List<char[]> ParseSignals(string[] input)
{
	//0 1 2 3 4 5 - 7 8 - 0
	
	// 7 segments: 8
	// 6 segments: 9,6,0
	// 5 segments: 2,3,5
	// 4	     : 4
	// 3         : 7
	// 2         : 1
	
	char[] digit1 = input.Single(x => x.Length == 2).ToCharArray();
	char[] digit7 = input.Single(x => x.Length == 3).ToCharArray();
	char[] digit4 = input.Single(x => x.Length == 4).ToCharArray();
	char[] digit8 = input.Single(x => x.Length == 7).ToCharArray();
	
	char[] digit6 = input.Single(x => x.Length == 6 && digit8.Except(x).Intersect(digit1).Count() == 1).ToCharArray();
	
	char topBar = digit7.Except(digit1).Single();
	char bottomBar = input.First(x => (x.Except(digit4).Except(digit7).Count() == 1)).Except(digit4).Except(digit7).Single();
	char leftBottomBar = digit8.Except(digit7).Except(digit4).Except(new[] { bottomBar}).Single();

	char[] digit9 = input.Single(x => AreEqual(x, digit7.Union(digit4).Union(new[] { bottomBar}))).ToCharArray();
	char[] digit0 = input.Single(x => x.Length == 6 && !AreEqual(x, digit9) && !AreEqual(x, digit6)).ToCharArray();
	
    char middleBar = digit8.Except(digit0).Single();
	
	char leftTopBar = digit4.Except(digit1).Except(new[] {middleBar}).Single();
	
	char[] digit5 = input.Single(x => x.Length == 5 && x.Contains(leftTopBar)).ToCharArray();
	
	char rightTopBar = digit4.Except(digit5).Single();
	char rightBottom = digit1.Except(new[] { rightTopBar}).Single();
	
	char[] digit2 = input.Single(x => x.Length==5 && !x.Contains(rightBottom)).ToCharArray();
	
	char[] digit3 = input.Single(x => x.Length==5 && x.Contains(rightTopBar) && x.Contains(rightBottom)).ToCharArray();
		
	return new List<char[]>() {digit0, digit1, digit2, digit3, digit4, digit5, digit6, digit7, digit8, digit9};
}

bool AreEqual(IEnumerable<char> a, IEnumerable<char> b)
{
	return a.Except(b).Count() == 0 && b.Except(a).Count() == 0;
}


