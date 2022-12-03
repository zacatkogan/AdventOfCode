namespace AdventOfCode
{
    public class Day_02 : BaseDay
    {
        Dictionary<string, RPS> opponent = new Dictionary<string, RPS>{
            { "A", RPS.Rock},
            { "B", RPS.Paper},
            { "C", RPS.Scissors}
        };

        Dictionary<RPS, RPS> outcomes = new()
        {
            { RPS.Rock, RPS.Paper },
            { RPS.Paper, RPS.Scissors },
            { RPS.Scissors, RPS.Rock}
        };



            






        int CalculateScore(RPS opponentPlay, RPS playerPlay)
        {
            if (opponentPlay == playerPlay)
                return 3 + (int)playerPlay;

            if ((opponentPlay == RPS.Rock && playerPlay == RPS.Paper)
                || (opponentPlay == RPS.Paper && playerPlay == RPS.Scissors)
                || (opponentPlay == RPS.Scissors && playerPlay == RPS.Rock))
                return 6 + (int)playerPlay;

            return (int)playerPlay;
        }

        RPS CalculateOutcome(RPS opponentPlay, string outcome)
        {
            if (outcome == "Y")
                return opponentPlay;
            
            if (outcome == "X")
                return Enum.GetValues<RPS>().Except(new RPS[]{opponentPlay, outcomes[opponentPlay]}).First();

            return outcomes[opponentPlay];
        }

        public override ValueTask<string> Solve_1()
        {
            Dictionary<string, RPS> player = new Dictionary<string, RPS>{
                { "X", RPS.Rock},
                { "Y", RPS.Paper},
                { "Z", RPS.Scissors}
            };

            int score = 0;

            foreach (var game in Data.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                var opponentPlay = opponent[game[0].ToString()];
                var playerPlay = player[game[2].ToString()];

                score += CalculateScore(opponentPlay, playerPlay);
            }
            
            return new(
                score
                + "," + 
                Solve_1_Clean()
                );
        }

        public override ValueTask<string> Solve_2()
        {
            int score = 0;

            foreach (var game in Data.Split("\n", StringSplitOptions.RemoveEmptyEntries))
            {
                var opponentPlay = opponent[game[0].ToString()];
                var desiredOutcome = game[2].ToString();

                var playerPlay = CalculateOutcome(opponentPlay, desiredOutcome);

                score += CalculateScore(opponentPlay, playerPlay);
            }

            return new(
                score
                + "," + 
                Solve_2_Clean()
                );
        }

        #region cleaner_implementations
        Dictionary<RPS, (RPS Beats, RPS Loses)> outcomeMap = new()
        {
            { RPS.Rock, (RPS.Scissors, RPS.Paper)},
            { RPS.Paper, (RPS.Rock, RPS.Scissors)},
            { RPS.Scissors, (RPS.Paper, RPS.Rock)}
        };

        int CalculateScore_Clean(RPS opponentPlay, RPS playerPlay)
        {
            if (opponentPlay == playerPlay)
                return (int)Outcome.Draw + (int)playerPlay;

            var outcome = outcomeMap[playerPlay];
            if (outcome.Beats == opponentPlay)
                return (int)Outcome.Win + (int)playerPlay;
            
            return(int)Outcome.Loss + (int)playerPlay;
        }

        RPS GetPlayerPlay_Clean(RPS opponentPlay, char outcome)
        {
            if (outcome == 'Y') // draw
                return opponentPlay;

            if (outcome == 'X') // loss
                return outcomeMap[opponentPlay].Beats;

            return outcomeMap[opponentPlay].Loses;
        }

        Dictionary<char, RPS> opponentMoveMap = new()
        {
            { 'A', RPS.Rock},
            { 'B', RPS.Paper},
            { 'C', RPS.Scissors}
        };

        public string Solve_1_Clean()
        {
            var opponentMoveMap = Enumerable.Zip(
                "ABC",
                Enum.GetValues<RPS>()
            ).ToDictionary(x => x.First, x => x.Second);

            var playerMoveMap = Enumerable.Zip(
                "XYZ",
                Enum.GetValues<RPS>()
            ).ToDictionary(x => x.First, x => x.Second);

            var result = Data.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => (opponent: x[0], player: x[2]))
                .Sum(x => CalculateScore(
                    opponentMoveMap[x.opponent],
                    playerMoveMap[x.player])
                );

            return result.ToString();
        }

        public string Solve_2_Clean()
        {
            var result = Data.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => (opponentMove: opponentMoveMap[x[0]], outcome: x[2]))
                .Select(x => (
                    x.opponentMove, 
                    playerMove:GetPlayerPlay_Clean(x.opponentMove, x.outcome)))
                .Sum(x => CalculateScore(x.opponentMove, x.playerMove));

            return result.ToString();
        }

        #endregion


        public enum RPS : int{
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }

        public enum Outcome : int
        {
            Win = 6,
            Draw = 3,
            Loss = 0
        }
    }
}
