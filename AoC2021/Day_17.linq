<Query Kind="Program" />

void Main()
{
		// at y = 0 for all starting +ve y velocities, the magnitude of the velocity will be the same.
		// i.e. if at `t = 0`, `vy = 100`, then after some steps `n` the projectile will cross the x-axis with the velocity `vy = -100`
		// therefore the maximum negative velocity it can have and still enter the target box is target_ymin at t = n+1
		// at t = n, the velocity must be `target_ymin - accel`, therefore the initial velocity must be `|target_ymin - accel|`
		
		// max height occurs when `vy_final = 0`, calculate py_final for `vy_init=131` and `accel = -1`
		
		// max height occurs after `-vy_initial/accel` +1 steps => 132 steps
			// x = 1/2(vf - vi)*t = 1/2*(131)*132 => 8646
		
		var target = new Target(155, 215, -132, -72);
		
		CalculateTrajectory(0, 131, target, out _);
		
		var vx_min = -1;
		var vx_max = 216;
		var vy_min = -133;
		var vy_max = 132;

		var count = 0;

		for(int i = vx_min; i <= vx_max; i++)
		for(int j = vy_min; j <= vy_max; j++)
		{
			if (CalculateTrajectory(i, j, target, out _))
				count++;
		}

		count.Dump();		
	}
	
	bool CalculateTrajectory(int vx, int vy, Target target, out List<(int x, int y)> points)
	{
		points = new List<(int x, int y)>();
		
		int px = 0;
		int py = 0;
	
		// if the projectile goes further down than the target, it's a miss.
		
		while (py >= target.yMin)
		{
			px += vx;
			if (vx > 0)
				vx--;
	
			py += vy;
			vy--;
					
			// check if we've hit
			if (target.PointIntersects(px, py))
				return true;
		}
	
		return false;
	}


record Target(int xMin, int xMax, int yMin, int yMax)
{
	public bool PointIntersects(int x, int y)
	{
		return x >= xMin && x <= xMax
			&& y >= yMin && y <= yMax;
	}
}
