<Query Kind="Statements" />

var source = File.ReadAllLines(MyExtensions.GetFilePathFromQueryDir("day5.txt"));

((int x, int y) start, (int x, int y) end) ParseCoords(string input)
{
	var raw = input.Split(" ");
	
	var rawStart = raw[0].Split(",");
	var rawEnd = raw[2].Split(",");
	
	return ((int.Parse(rawStart[0]), int.Parse(rawStart[1])), (int.Parse(rawEnd[0]), int.Parse(rawEnd[1])));
}

var coords = source.Select(ParseCoords);

// part 1:
var vents = new int [1000,1000];

// only straight vents for part 1
foreach (var vent in coords.Where(v => v.start.x == v.end.x || v.start.y == v.end.y))
{
	var deltax = vent.end.x - vent.start.x;
	var deltay = vent.end.y - vent.start.y;
	
	for (int i = 0; Math.Abs(i) <= Math.Abs(deltax); i = (deltax >= 0 ? i+1 : i-1))
	for (int j = 0; Math.Abs(j) <= Math.Abs(deltay); j = (deltay >= 0 ? j+1 : j-1))
	{
		vents[vent.start.x + i, vent.start.y + j]++;
	}
}

var sum = 0;

for (int i = 0; i < 1000; i++)
for (int j = 0; j < 1000; j++)
{
	if (vents[i,j] > 1)
		sum++;
}

sum.Dump();









var vents2 = new int [1000,1000];

// only straight vents for part 1

foreach (var vent in coords)
{
	var deltax = vent.end.x - vent.start.x;
	var deltay = vent.end.y - vent.start.y;

	if (deltax == 0 || deltay == 0)
	{
		for (int i = 0; Math.Abs(i) <= Math.Abs(deltax); i = (deltax >= 0 ? i + 1 : i - 1))
			for (int j = 0; Math.Abs(j) <= Math.Abs(deltay); j = (deltay >= 0 ? j + 1 : j - 1))
			{
				vents2[vent.start.x + i, vent.start.y + j]++;
			}
	}
	else if (Math.Abs(deltax) == Math.Abs(deltay))
	{
		int xdir = deltax < 0 ? -1 : 1;
		int ydir = deltay < 0 ? -1 : 1;
		
		for (int i = 0; i <= Math.Abs(deltax); i++)
		{
			try
			{
				vents2[vent.start.x + (xdir * i), vent.start.y + (ydir * i)]++;
			}
			catch
			{}
		}
	}
	else
	{
		throw new Exception();
	}
}

var sum2 = 0;

for (int i = 0; i < 1000; i++)
	for (int j = 0; j < 1000; j++)
	{
		if (vents2[i, j] > 1)
			sum2++;
	}

sum2.Dump();