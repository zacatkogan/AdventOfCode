<Query Kind="Program" />

void Main()
{
	//string[] rawInput = @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#
	//
	//#..#.
	//#....
	//##..#
	//..#..
	//..###".Split("\r\n");

	string[] rawInput = File.ReadAllLines(MyExtensions.GetFilePathFromQueryDir("day20.txt"));

	// N.B.: Array Indexing: array[row, col]


	algoLookup = rawInput.First();
	List<string> mapRaw = rawInput.Skip(2).ToList();

	var initMap = InitializeMap(mapRaw);
	Part1(initMap);
	Part2(initMap);
}

void Part1(int[,] map)
{
	map = EmbiggenMap(map, 4);//.Dump();

	map = EnhanceMap(map, 2);
	Count(map, 2).Dump("Part 1");
}

void Part2(int[,] map)
{
	// need to pad to at least twice as many as 'enhance iterations'
	map = EmbiggenMap(map, 100);
	map = EnhanceMap(map, 50);
	
	// since we need to remove that many outer shells as well to account for interactions with the infinite plane.
	Count(map, 50).Dump("Part 2");
}

List<(int j, int i)> elementIndexes = (from i in Enumerable.Range(-1, 3)
					  from j in Enumerable.Range(-1, 3)
					  select (j, i)
					 ).ToList();
					 
string algoLookup;

int[,] InitializeMap(List<string> str)
	{
		var xDim = str[0].Length;
		var yDim = str.Count;

		var initMap = new int[xDim, yDim];
		for (int j = 0; j < yDim; j++)
			for (int i = 0; i < xDim; i++)
			{
				initMap[i, j] = str[i][j] == '.' ? 0 : 1;
			}

		return initMap;
	}

int[,] EnhanceMap(int[,] map)
{
	var newMap = new int[map.GetLength(0), map.GetLength(1)];

	for (int i = 0; i < map.GetLength(1); i++)
	{
		for (int j = 0; j < map.GetLength(0); j++)
		{
			var elementValues = elementIndexes
				.Select(e => (i: i + e.i, j: j + e.j)) // convert to absolute index values
				.Select(v =>
				{
					if (v.i < 0 || v.j < 0 || v.i > map.GetUpperBound(1) || v.j > map.GetUpperBound(0))
						return 0;
					return map[v.i, v.j];
				})                                     // get the numeric value of each index
				.Select(x => x.ToString());            // convert to string
			var rawBinary = string.Join("", elementValues);
			int lookupValue = Convert.ToInt32(rawBinary, 2);

			var newValue = algoLookup[lookupValue] == '.' ? 0 : 1;
			newMap[i, j] = newValue;
		}
	}

	return newMap;
}

int[,] EnhanceMap(int[,] map, int iterations)
{
	for (int i = 0; i < iterations; i++)
	{
		map = EnhanceMap(map);
	}
	
	return map;
}

// expands the original map in each direction by one unit
int[,] EmbiggenMap(int[,] map, int buffer)
{
	var oldMapYMax = map.GetLength(0);
	var oldMapXMax = map.GetLength(1);

	// add 2 for padding
	var newMapYMax = oldMapYMax + 2*buffer; 
	var newMapXMax = oldMapXMax + 2*buffer;

	var newMap = new int[newMapYMax, newMapXMax];

	for (int x = 0; x < map.GetLength(0); x++)
	{
		for (int y = 0; y < map.GetLength(1); y++)
		{
			// old map gets inserted starting at 1,1
			newMap[x + buffer, y + buffer] = map[x, y];
		}
	}

	return newMap;
}

// counts the number of bright pixels, ignoring the outermost `trim` count of rows/columns
int Count(int[,] map, int trim = 0)
{
	int ctr = 0;
	
	for (int x = trim; x < map.GetLength(0) - trim; x++)
	{
		for (int y = trim; y < map.GetLength(1) - trim; y++)
		{
			if (map[x, y] != 0)
				ctr++;
		}
	}
	
	return ctr;
}