<Query Kind="Program" />

void Main()
{
//	var raw = @"1163751742
//1381373672
//2136511328
//3694931569
//7463417111
//1319128137
//1359912421
//3125421639
//1293138521
//2311944581".Split("\r\n");

	var raw = File.ReadAllLines(MyExtensions.GetFilePathFromQueryDir("day15.txt"));
	var map = raw.Select(x => x.Select(y => int.Parse(y.ToString())).ToArray()).ToArray();
	
	GetLowestRisk(map, 1).Dump("part 1");
	GetLowestRisk(map, 5).Dump("part 2");
}

List<(int x, int y)> neighbors = new List<(int x, int y)>
	{
		(-1, 0), (1, 0), (0, -1), (0, 1)
	};

public int GetLowestRisk(int[][] rawMap, int scaling)
{
	var xMax = rawMap[0].Count() * scaling - 1;
	var yMax = rawMap.Count() * scaling - 1;

	var map = new ScaledMap(rawMap, 1, 0);

	C finish = new C(xMax, yMax);

	var q = new PriorityQueue<C, int>();
	q.Enqueue(new C(0, 0), 0);

	var visited = new Dictionary<C, int>();

	while (q.TryDequeue(out C c, out int p))
	{
		if (c == finish)
		{
			return p;
		}

		if (!visited.TryAdd(c, p))
			continue;

		var nodes = neighbors.Select(n => new C(c.X + n.x, c.Y + n.y))
			.Where(n => n.X >= 0 && n.X <= xMax
					 && n.Y >= 0 && n.Y <= yMax)
			.Where(n => !visited.ContainsKey(n))
			.ToList();

		foreach (var node in nodes)
		{
			// the total cost of visiting a node is the cost of the previous node plus its own cost
			var newPriority = map[node.X, node.Y] + p;
			q.Enqueue(node, newPriority);
		}
	}
	
	throw new Exception();
}

public record C(int X, int Y);

public class Map
{
	public Map(int[][] map)
	{
		array = map;
	}
	
	protected int[][] array;
	
	public virtual int this[int x, int y]
	{
		get { return array[x][y]; }
	}
}

public class ScaledMap : Map
{
	int Scaling;
	int xDim;
	int yDim;
	
	public ScaledMap(int[][] map, int scaling, int cost) : base(map)
	{
		Scaling = scaling;
		
		xDim = map[0].Length * scaling;
		yDim = map.Length * scaling;
	}

	public override int this[int x, int y]
	{
		get {
			var baseCost = base[x % xDim, y % yDim];
			var xTiles = x/xDim;
			var yTiles = y/yDim;
			
			var newCost = baseCost + xTiles + yTiles;
			if (newCost > 9)
				newCost = (newCost -1) % 9 + 1;
			
			
			return newCost;
		}
	}



}