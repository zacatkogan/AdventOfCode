using AdventOfCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2018
{
    internal class Day07 : BaseDay
    {
        public override object Solve1()
        {
            var steps = ParseDag();

            var completedSteps = new List<Step>();
            var completedSet = new HashSet<Step>();
            var eligibleSteps = new SortedList<string, Step>();

            var firstSteps = steps.Values.Where(x => !x.Requirements.Any()).ToList();
            firstSteps.ForEach(s => eligibleSteps.Add(s.StepName, s));

            while (eligibleSteps.Any())
            {
                var evaluatingStep = eligibleSteps.First();
                eligibleSteps.RemoveAt(0);
                var s = evaluatingStep.Value;

                completedSteps.Add(evaluatingStep.Value);
                completedSet.Add(evaluatingStep.Value);

                foreach (var unblock in s.Unblocks)
                {
                    if (unblock.Requirements.All(x => completedSet.Contains(x)))
                        eligibleSteps.Add(unblock.StepName, unblock);
                }
            }

            return string.Join("", completedSteps.Select(s => s.StepName));
        }

        public override object Solve2()
        {
            var steps = ParseDag();

            var jobs = new List<Job>();

            var completedSteps = new List<Step>();
            var completedSet = new HashSet<Step>();
            var eligibleJobs = new SortedList<string, Job>();

            steps.Values.Where(x => !x.Requirements.Any()).ToList()
                .ForEach(x => eligibleJobs.Add(x.StepName, new Job(x)));

            var currentTick = 0;

            while (jobs.Any() || eligibleJobs.Any())
            {
                // fill up Jobs from eligible jobs
                eligibleJobs.Take(5 - jobs.Count).ToList()
                    .ForEach(j =>
                    {
                        jobs.Add(j.Value);
                        eligibleJobs.Remove(j.Key);
                        //Console.WriteLine($"Tick: {currentTick}: Beginning Job {j.Key}");
                    });

                var minTicks = jobs.Min(x => x.RemainingTime);
                jobs.ForEach(j => j.RemainingTime -= minTicks);
                currentTick += minTicks;

                // trim any completed jobs
                var finishedJobs = jobs.Where(x => x.RemainingTime <= 0).ToList();
                foreach (var j in finishedJobs)
                {
                    var step = j.CurrentTask;
                    //Console.WriteLine($"Tick: {currentTick}: Finished Job {step.StepName}");
                    completedSteps.Add(step);
                    completedSet.Add(step);
                    foreach (var u in step.Unblocks)
                        if (!completedSet.Contains(u) && u.Requirements.All(x => completedSet.Contains(x)))
                            eligibleJobs.TryAdd(u.StepName, new Job(u));
                    jobs.Remove(j);
                }
            }

            return currentTick;
        }


        public class Job
        {
            public Step CurrentTask;
            public int RemainingTime;

            public Job(Step step)
            {
                CurrentTask = step;
                RemainingTime = 60 + (int)(step.StepName[0] - 'A') + 1;
            }

            public void Step(int time = 1)
            {
                RemainingTime -= time;
            }
        }

        public Dictionary<string, Step> ParseDag()
        {
            var regex = new Regex(@"Step (?<priorStep>\w) must be finished before step (?<step>\w) can begin.");

            var deps = new List<(string step, string priorStep)>();

            foreach (var i in DataLines)
            {
                var matches = regex.Matches(i);
                var groups = matches.First().Groups;
                deps.Add((groups["step"].Value, groups["priorStep"].Value));
            }

            //	deps.Dump();

            // generate the list of Steps
            Dictionary<string, Step> steps =
                deps.Select(x => x.step)
                    .Union(deps.Select(d => d.priorStep))
                    .Distinct()
                    .ToDictionary(
                        d => d,
                        d => new Step { StepName = d });

            foreach (var dep in deps)
            {
                var currentStep = steps[dep.Item1];
                var currentDep = steps[dep.Item2];

                currentStep.Requirements.Add(currentDep);
                currentDep.Unblocks.Add(currentStep);
            }

            return steps;
        }

        public record struct Step
        {
            public Step() { }
            public string StepName { get; set; }
            public List<Step> Requirements { get; set; } = new();
            public List<Step> Unblocks { get; set; } = new();
        }
    }
}
