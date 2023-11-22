<Query Kind="Program" />

void Main()
{
	int[] playerPosition = new[] {0,0};
	int[] playerScore = new[] {0,0};
	bool currentPlayer = false;
	
	InitPosition();
	
	while (!playerScore.Any(p => p >= 1000))
	{
		var distance = RollDie3Times() % 10;
		
		var p = currentPlayer ? 1 : 0;
		
		playerPosition[p] = ((playerPosition[p] + distance - 1) % 10)+1;
		playerScore[p] += playerPosition[p];
		
		currentPlayer = !currentPlayer;
	}

	(playerScore.Min() * die).Dump();
	
	int RollDie()
	{
		return ((++die - 1) % 100) + 1;
	}
	
	int RollDie3Times()
	{
		return RollDie() + RollDie() + RollDie();
	}
	
	void InitPosition()
	{
		playerPosition = new[] {10,8};
		playerScore = new[] {0,0};
	}
}

int die = 0;