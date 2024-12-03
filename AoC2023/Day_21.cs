namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using System.ComponentModel;

    public class Day_21 : BaseDay
    {
        public static int currentPress = 0;

        #region Part1
        public override object Solve1()
        {
            HashSet<Position> garden = new();

            Position start = new Position();

            var map = DataLines;
            for (int row = 0; row < map.Length; row++)
            {
                var r = map[row];
                for (int col = 0; col < r.Length; col++)
                {
                    if (r[col] == '.')
                        garden.Add((col, row));
                    if (r[col] == 'S')
                    {
                        garden.Add((col, row));
                        start = ((col, row));
                    }
                }
            }

            
            var currentStep = new HashSet<Position>();
            var prevStep = new HashSet<Position>() { start };

            for (int step = 0; step < 64; step++)
            {
                foreach (var s in prevStep)
                {
                    foreach (var n in Position.Directions.Values)
                    {
                        var newStepCandidate = s + n;
                        if (garden.Contains(newStepCandidate))
                            currentStep.Add(newStepCandidate);
                    }
                }

                prevStep = currentStep;
                currentStep = new();
            }

            return prevStep.Count;
        }
        #endregion

        #region Part2
        public override object Solve2()
        {
            //Data = "...........\n.....###.#.\n.###.##..#.\n..#.#...#..\n....#.#....\n.##..S####.\n.##..#...#.\n.......##..\n.##.#.####.\n.##..##.##.\n...........";
            //DataLines = null;


            var mapHeight = DataLines.Length;
            var mapWidth = DataLines[0].Length;

            HashSet<Position> garden = new();
            Position start = new Position();

            var map = DataLines;
            for (int row = 0; row < map.Length; row++)
            {
                var r = map[row];
                for (int col = 0; col < r.Length; col++)
                {
                    if (r[col] == '.')
                        garden.Add((col, row));
                    if (r[col] == 'S')
                    {
                        garden.Add((col, row));
                        start = ((col, row));
                    }
                }
            }

            var currentStep = new HashSet<Position>();
            var prevStep = new HashSet<Position>() { start };

            List<(int, long)> matches = new();

            for (int step = 1; step < 20000; step++)
            {
                if (matches.Count == 3)
                    break;
                foreach (var s in prevStep)
                {
                    foreach (var n in Position.Directions.Values)
                    {
                        var newStepCandidate = s + n;
                        Position boundedStepCandidate = (newStepCandidate.X % mapWidth, newStepCandidate.Y % mapHeight);
                        if (boundedStepCandidate.X < 0)
                            boundedStepCandidate.X += mapWidth;
                        if (boundedStepCandidate.Y < 0)
                            boundedStepCandidate.Y += mapHeight;

                        if (garden.Contains(boundedStepCandidate))
                            currentStep.Add(newStepCandidate);
                    }
                }

                prevStep = currentStep;

                if (step % 262 == 65) // 65 is 25601365 % 262
                {
                    Console.WriteLine(prevStep.Count);
                    matches.Add((step, prevStep.Count));
                }
                currentStep = new();
            }

            // at steady state, alternates between 7325 7265 after approx 130 steps.


            // too low: 149274256870980
            //          149277208340440
            //          149277228368254 // wrong - no help
            //          608152828731262
            //          289356730553132 // wrong, no help
            //          144678365276566
            // 597102953699891

            var extrapolateSteps = 26501365 / 262;

            var stepCount = matches[0].Item1;
            long stepIncrement = 0L;
            long count = matches[0].Item2;

            var startCount = matches[0].Item2;
            var secondCount = matches[1].Item2;
            var thirdCount = matches[2].Item2;

            var step1Diff = secondCount - startCount;
            var step2Diff = thirdCount - secondCount;

            var increment = step2Diff - step1Diff;
            stepIncrement = step1Diff;

            while (stepCount < 26501365)
            {
                stepCount += 262;
                count += stepIncrement;
                stepIncrement += increment;
            }
            



            return prevStep.Count;
        }
        #endregion
    }
}
