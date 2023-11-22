<Query Kind="Statements" />

IEnumerable<int> diceRolls =
   (from i in Enumerable.Range(1, 3)
	from j in Enumerable.Range(1, 3)
	from k in Enumerable.Range(1, 3)
	select (i + j + k)).ToList();

IEnumerable<int> distinctDiceRolls = diceRolls.Distinct().Dump();
Dictionary<int, int> diceRollDistributions = diceRolls
	.GroupBy(x => x)
	.ToDictionary(x => x.Key, x => x.Count())
	.Dump();

var p1Moves = CalculateAllPossibleMovesForStartingPos(10)
.ToLookup(x => x.turn);

var p2Moves = CalculateAllPossibleMovesForStartingPos(8)
.ToLookup(x => x.turn);

decimal p1runningtotal = 0;
decimal p2runningtotal = 0;

// earliest win is at turn = 3, latest game ends after 10 turns.
for (int i = 3; i <= 10; i++)
{
// check for p1 wins, but p2 hasn't one on the previous turn
p1runningtotal += CrossMultiply(
		p1Moves[i].Where(x => x.score >= 21).ToList(), 
		p2Moves[i-1].Where(x => x.score < 21).ToList(),
		(x,y) => (p1:x, p2: y))
	.Select(x => (decimal)x.p1.numOutcomes * (decimal)x.p2.numOutcomes)
	.Sum();

p2runningtotal += CrossMultiply(
		p2Moves[i].Where(x => x.score >= 21).ToList(),
		p1Moves[i].Where(x => x.score < 21).ToList(),
		(x,y) => (p2:x, p1: y))
	.Select(x => (decimal)x.Item1.numOutcomes * (decimal)x.Item2.numOutcomes)
	.Sum();
}

p1runningtotal.Dump("p1");
p2runningtotal.Dump("p2");

Math.Max(p1runningtotal, p2runningtotal).Dump("winner");

List<(int turn, int score, int numOutcomes)> CalculateAllPossibleMovesForStartingPos(int startingPos)
{
	List<(int pos, int score, int numOutcomes)> outcomes = new() { (startingPos, 0, 1) };
	List<(int turn, int score, int numOutcomes)> moves = new();

	var turns = 0;
	while (outcomes.Any())
	{
		turns++;
		outcomes = (from o in outcomes
					from r in distinctDiceRolls
					select CalculateNewOutcome(o.pos, o.score, o.numOutcomes, r)
				   ).ToList();

		// add current outcomes to the list of 
		moves.AddRange(outcomes.Select(x => (turns, x.score, x.numOutcomes)));

		// check if we've got completed games, and remove them from the list of future tests
		outcomes.RemoveAll(x => x.score >= 21);
	}

	return moves;
}

IEnumerable<TOut> CrossMultiply<TIn, TOut>(IEnumerable<TIn> t1, IEnumerable<TIn> t2, Func<TIn, TIn, TOut> selector)
{
	return from p1 in t1
		   from p2 in t2
		   select selector(p1, p2);
}

int ClipValue(int value, int max) => ((value - 1) % max) + 1;
int ClipTo10(int value) => ClipValue(value, 10);

(int pos, int score, int numOutcomes) CalculateNewOutcome(int pos, int score, int numOutcomes, int roll)
{
	var newPos = ClipTo10(pos + roll);
	var newScore = score + newPos;

	return (newPos, newScore, numOutcomes * diceRollDistributions[roll]);
}