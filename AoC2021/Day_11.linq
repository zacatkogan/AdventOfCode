<Query Kind="Statements" />

var rawInput = File.ReadAllLines(MyExtensions.GetFilePathFromQueryDir("day11.txt"));

int[][] data = rawInput.Select(x => x.ToCharArray()
    .Select(x => int.Parse(x.ToString())).ToArray()).ToArray();//.Dump();
	
var directions = 
(from x in Enumerable.Range(-1, 3)
from y in Enumerable.Range(-1, 3)
select (x, y))
.ToList();

checked
{
	Enumerable.Range(0, 100)
	.Select(e => Step(data))
	.Sum()
	.Dump("part 1");
}

// Data starts from Step 100
var step = 100;
while (true)
{
	step++;
	
	if (Step(data) == 100)
	{
		step.Dump("part 2");
		break;
	}	
}

int Step(int[][] data)
{
	// jellyfish that need to get recalculated because a neighboring jellyfish flashed
	var toVisit = new Stack<(int x, int y)>();

	// jellyfish that already flashed and can't get flashed again
	var flashed = new HashSet<(int x, int y)>();

	// increment all jellyfish, and look for all the jellyfish that seed the flashing
	for(int i = 0; i < 10; i++)
	for(int j = 0; j < 10; j++)
	{
		var temp = ++data[i][j];
		if (temp == 10)
		{
			directions.ForEach(d => toVisit.Push((i + d.x, j + d.y)));
			flashed.Add((i, j));
		}
	}

	while (toVisit.TryPop(out (int x, int y) result))
	{
		if (flashed.Contains(result))
			continue;

		//// index out of bounds
		//if (result.x < 0 || result.y < 0 || result.x > 9 || result.y > 9)
		//	continue;

		try
		{
			var temp = ++data[result.x][result.y];

			if (temp == 10)
			{
				directions.ForEach(d => toVisit.Push((result.x + d.x, result.y + d.y)));
				flashed.Add((result.x, result.y));
			}
		}
		catch (IndexOutOfRangeException e){} // cbf doing range checking
	}
	
	// reset the jellyfish that flashed to 0
	foreach (var result in flashed)
	{
		data[result.x][result.y] = 0;
	}
	
	return flashed.Count;
}