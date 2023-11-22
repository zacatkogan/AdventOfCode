<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
</Query>

var rawInput = File.ReadAllLines(MyExtensions.GetFilePathFromQueryDir("Day13.txt")).ToList();

// find the empty line that splits the data
var splitIndex = rawInput.Select((x, i) => (index: i, content: x)).First(x => string.IsNullOrEmpty(x.content)).index;

var dots = rawInput.Take(splitIndex)
	.Select(i => i.Split(","))
	.Select(i => new Dot { x = int.Parse(i[0]), y = int.Parse(i[1])})
	.ToList();

var folds = rawInput.Skip(splitIndex + 1).ToList();

// Part 1
ApplyFold(dots, folds.First());
dots.Distinct().Count().Dump("part 1");

// Part 2, building on Part 1
foreach(var f in folds.Skip(1))
	ApplyFold(dots, f);

// converting to an image
var bmp = new Bitmap(50, 10);
foreach (var dot in dots)
	bmp.SetPixel(dot.x, dot.y, Color.Black);

// scale and output
new Bitmap(bmp, bmp.Width * 3, bmp.Height * 3).Dump("part 2");

void ApplyFold(List<Dot> dots, string fold)
{
	var location = fold.Split(" ").Last().Split("=");
	var dir = location[0];
	var line = int.Parse(location[1]);

	dots.ForEach(d => {
		if (dir == "x" && d.x > line)
			d.x = 2 * line - d.x;
		if (dir == "y" && d.y > line)
			d.y = 2 * line - d.y;
	});
}

public record Dot
{
	public Dot() { }
	public Dot((int x, int y) tuple)
	{
		x = tuple.x;
		y = tuple.y;
	}
	public int x {get; set;}
	public int y { get; set;}
}