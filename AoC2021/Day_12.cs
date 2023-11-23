<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Collections.Specialized</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

string[] rawInput = File.ReadAllLines(MyExtensions.GetFilePathFromQueryDir("day12.txt"));

Random globalRand = new Random();

void Main()
{
	// start: 350ms
	// visitedSmall: 290-300
	// validNodes.RemoveAll: 200ms
	// lazily allocate valid nodes on TwoVisitsYet = true: 165ms
	// local rand: 160ms
	
	// 1M takes 2s
	// 10M takes 20s
	// 100M takes 200s (3.3m)
	// 1B takes 2000s (33m)
	// 10B takes 20_000s (5.5h)
	//"1B iterations".Dump();
	//MyExtensions.Time(() => { Part2Random(1_000_000_000);});
	//
	//"10B iterations".Dump();
	//MyExtensions.Time(() => { Part2Random(10_000_000_000);});




	MyExtensions.Time(() => { Part2Random(100_000);});
	MyExtensions.Time(() => { Part2Random(100_000);});
	MyExtensions.Time(() => { Part2Random(100_000);});
	MyExtensions.Time(() => { Part2Random(100_000);});



}

public int Part2Random(long iterations)
{
	var graph = ConstructGraph(rawInput);//.Dump();

	var start = graph["start"];
	var finish = graph["end"];

	// remove the Start node from all nodes - we can't travel back to this.
	foreach (var n in graph.Values) { n.Connections.Remove(start); }

	var routes = new ConcurrentDictionary<string, string>();

	int errors = 0;

	//Parallel.For(0, (int)(iterations / 1_000_000),
	//(iter) =>
	//{
		// create a local Rand
		Random rand = new Random(this.globalRand.Next());
		
		for (long i = 0; i < iterations; i++)
		{
			var path = new List<Node>();
			var visitedSmall = new HashSet<Node>();

			path.Add(start);

			var currentNode = start;
			var twoVisitsYet = false;

			while (currentNode != finish)
			{
				List<Node> validNodes;

				if (twoVisitsYet)
				// get all nodes, except for small caves that have been visited
				{
					validNodes = currentNode.Connections.ToList();
					validNodes.RemoveAll(x => visitedSmall.Contains(x));
				}
				else
				{
					validNodes = currentNode.Connections;
				}

				if (validNodes.Count == 0)
				{
					// this is an error
					goto end;
				}
				else
				{
					// pick one at random
					currentNode = validNodes[rand.Next(validNodes.Count)];

					bool isSmall = (currentNode.Size == Size.Small);

					// if this is a small cave, check if we've already visited
					if (isSmall)
					{
						if (!twoVisitsYet && visitedSmall.Contains(currentNode))
						{ twoVisitsYet = true; }

						visitedSmall.Add(currentNode);
					}

					path.Add(currentNode);
				}
			}
			var routeString = string.Join("", path.Select(x => x.Name));
			routes.TryAdd(routeString, routeString);

		end:;
		}
		//});
//	errors.Dump("errors");
	routes.Count.Dump("DistinctRoutes");
	return routes.Count;
}

public void Part1()
{
	var graph = ConstructGraph(rawInput);//.Dump();

	var routes = new List<string>();
	int errors = 0;
	// run a monte-carlo simulation to get all nodes from Start to Finish

	var start = graph["start"];
	var finish = graph["end"];

	for (int i = 0; i < 10_000_000; i++)
	{
		var path = new List<Node>();

		path.Add(start);

		var currentNode = start;

		while (currentNode != finish)
		{
			// get all nodes, except for small caves that have been visited
			var validNodes = currentNode.Connections.Except(path.Where(x => x.Size == Size.Small)).ToList();

			if (validNodes.Count == 0)
			{
				// this is an error
				errors++;
				goto end;
			}
			else
			{
				// pick one at random
				currentNode = validNodes[globalRand.Next(validNodes.Count)];
				path.Add(currentNode);
			}
		}

		routes.Add(string.Join("", path.Select(x => x.Name)));

	end:;
	}
	errors.Dump("errors");
	routes.Count().Dump("Pre-unique count");
	var distinctRoutes = routes.Distinct().ToList();
	distinctRoutes.Count.Dump("DistinctRoutes");
}

// You can define other methods, fields, classes and namespaces here

public class Node
{
	public Node(string name)
	{
		Name = name;
		Size = name.ToLower() == name ? Size.Small : Size.Big;
	}
	
	public string Name {get;set;}
	public Size Size {get;set;}
	
	public List<Node> Connections {get;} = new List<Node>();
}

public enum Size
{
	Big,
	Small
}

public IDictionary<string, Node> ConstructGraph(string[] connections)
{
	ConcurrentDictionary<string, Node> nodes = new ConcurrentDictionary<string, Node>();
	
	foreach (var connection in connections)
	{
		var conns = connection.Split('-');
		var first = conns[0];
		var second = conns[1];
		
		var n1 = nodes.GetOrAdd(first, new Node(first));
		var n2 = nodes.GetOrAdd(second, new Node(second));
		n1.Connections.Add(n2);
		n2.Connections.Add(n1);
	}
	
	return nodes;
}

public class Route
{
	
}

