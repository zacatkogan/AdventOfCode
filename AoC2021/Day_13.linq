<Query Kind="Statements" />

var rawInput = File.ReadAllLines(MyExtensions.GetFilePathFromQueryDir("Day13.txt")).ToList();

var splitIndex = rawInput.Select((x, i) => (index: i, content:x)).First(x => string.IsNullOrEmpty(x.content)).index;

var dots = rawInput.Take(splitIndex).Select(i => i.Split(",")).Select(i => (x: int.Parse(i[0]),y: int.Parse(i[1]))).ToList();
dots.Count().Dump("no dots");

var folds = rawInput.Skip(splitIndex+1).ToList();

int xLength = dots.Select(d => d.x).Max()+1;
int yLength = dots.Select(d => d.y).Max()+1;

var field = new int[xLength,yLength];

// populate field
dots.ForEach(d => field[d.x, d.y] = 1);

field = ApplyFold(field, folds.First());

int count = 0;
for (int i = 0; i <= field.GetUpperBound(0); i++)
for (int j = 0; j <= field.GetUpperBound(1); j++)
	if (field[i,j] == 1) count++;
	
count.Dump("part1");

foreach (var fold in folds.Skip(1))
{
	field = ApplyFold(field, fold);
}


int[,] TransposeMatrix(int[,] matrix)
{
	var rows = matrix.GetLength(0);
	var columns = matrix.GetLength(1);

	var result = new int[columns, rows];

	for (var c = 0; c < columns; c++)
	{
		for (var r = 0; r < rows; r++)
		{
			result[c, r] = matrix[r, c];
		}
	}

	return result;
}


TransposeMatrix(field).Dump();


int[,] ApplyFold(int[,] field, string fold)
{
	var location = fold.Split(" ").Last().Split("=");
	var dir = location[0];
	var line = int.Parse(location[1]);
	
	var newXLength = xLength;
	var newYLength = yLength;
	
	if (dir == "x")
		newXLength = line; // assumes that Line > .5 * length
	else
		newYLength = line; // assumes that Line > .5 * length

	var newField = new int[newXLength,newYLength];
	
	// copy to new array, mirroring around the end of the array dimension
	for(int i = 0; i < xLength; i++)
	for(int j = 0; j < yLength; j++)
	{
		var tmpX = i;
		var tmpY = j;
		
		// i = 999, newXLength = 700:
			// i - newXLength == 299
			// newXLength - (i - newXLength) = 700-299 = 401
			// == 2 * newXLentgh - i
		
		// if it exceeds the new dimension, mirror it back
		if (i > newXLength)
			tmpX = 2 * newXLength - i;
		if (j > newYLength)
			tmpY = 2 * newYLength - j;
		
		if (field[i,j] == 1)
			newField[tmpX, tmpY] = 1;
	}
	
	xLength = newXLength;
	yLength = newYLength;
	return newField;
}