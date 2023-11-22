<Query Kind="Statements" />

var inputRaw = File.ReadAllText(MyExtensions.GetFilePathFromQueryDir("Day3.txt"));

var input = inputRaw.Split(new[] { "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);

var binaryInputs = input.Select(s => s.ToArray().Select(c => int.Parse(c.ToString())).ToArray());

int IntArrayAsBinaryInt(IEnumerable<int> input) => Convert.ToInt32(string.Join("", input), 2);

// PART 1
var part1output = Enumerable.Range(0,12) 
	.Select(												// iterate over the 12 digits
		x => binaryInputs
			.GroupBy(b => b[x]) 							// group by digit
			.Select(b => (bit: b.Key, count: b.Count())) 	// count occurrences of digits
			.OrderByDescending(b => b.count) 				// sort by descending frequency
			.Select(x => x.bit)								// 
			.First()										// get the value
	);

var gammaRate = IntArrayAsBinaryInt(part1output).Dump("gamma rate");
var epsilonRate = IntArrayAsBinaryInt(part1output.Select(x => x == 1 ? 0 : 1 )).Dump("epsilon rate");

(gammaRate * epsilonRate).Dump("part1 output");

// PART 2
// oxygen rating
int oxygenRating = 0;

List<int[]> rating = binaryInputs.ToList();

for (int i = 0; i < 12; i++)
{
	var bit = rating.GroupBy(b => b[i])                 // group by digit
		.Select(b => (bit: b.Key, count: b.Count()))    // count occurrences of digits
		.OrderByDescending(b => b.count + 0.5 * b.bit)  // sort by descending frequency, favoring 1s over 0s in the event of a tie
		.Select(x => x.bit)                             // 
		.First();
	
	// trim the resultant list based on the most prevalent bit
	rating = rating.Where(x => x[i] == bit).ToList();

	if (rating.Count == 1)
	{
		var rawRating = rating[0];
		oxygenRating = IntArrayAsBinaryInt(rawRating).Dump("oxygen rating");
		break;
	}
}

// reset the rating list
int co2rating = 0;

rating = binaryInputs.ToList();

for (int i = 0; i < 12; i++)
{
	var bit = rating.GroupBy(b => b[i])                 // group by digit
		.Select(b => (bit: b.Key, count: b.Count()))    // count occurrences of digits
		.OrderBy(b => b.count + 0.5 * b.bit)  			// sort by ascending frequency, favoring 0s over 1s in the event of a tie
		.Select(x => x.bit)                             // 
		.First();

	// trim the resultant list based on the most prevalent bit
	rating = rating.Where(x => x[i] == bit).ToList();

	if (rating.Count == 1)
	{
		var rawRating = rating[0];
		co2rating = IntArrayAsBinaryInt(rawRating).Dump("co2 rating");
		break;
	}
}

(oxygenRating * co2rating).Dump("part2 output");
