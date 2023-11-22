<Query Kind="Program" />

IEnumerable<int> source = File.ReadAllText(MyExtensions.GetFilePathFromQueryDir("day6.txt")).Split(",").Select(int.Parse);//.Dump();

void Main()
{
	Part1();
	Part2();
	
	return;
}

public void Part1()
{
	// part 1
	List<byte> LanternFish = source.Select(x => (byte)x).ToList();

	try
	{
		for (int d = 0; d < 80; d++)
		{
			int count = LanternFish.Count;

			for (int i = 0; i < count; i++)
			{
				var fish = LanternFish[i];
				if (fish == 0)
				{
					LanternFish[i] = 6;
					LanternFish.Add(8);
				}
				else
				{
					LanternFish[i]--;
				}
			}
		}
		LanternFish.Count.Dump();
	}
	catch (Exception e)
	{
		e.Dump();
	}
}

// part 2: bucketize and iterate
public void Part2()
{
	var lanternFishGrouped = source.GroupBy(x => x).ToDictionary(x => x.Key,x => (long)x.Count());
	
	for (int d = 0; d < 256; d++)
	{
		// decrement...
		var tmp = lanternFishGrouped.ToDictionary(x => x.Key - 1, x => x.Value);

		// check for "new fish"
		if (tmp.TryGetValue(-1, out long newFish))
		{
			tmp.Remove(-1);
			if (tmp.ContainsKey(6))
				tmp[6] = tmp[6] + newFish;
			else
				tmp.Add(6, newFish);
			tmp.Add(8, newFish);
		}
		
		lanternFishGrouped = tmp;
	}
	
	lanternFishGrouped.Sum(x=> (long)x.Value).Dump();
}