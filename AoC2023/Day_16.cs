namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading.Tasks.Dataflow;
    using Beam = (Position location, Position direction);

    public class Day_16: BaseDay
    {
        public static Position South = new Position(0, 1);
        public static Position North = new Position(0, -1);
        public static Position West = new Position(-1, 0);
        public static Position East = new Position(1, 0);

        public Dictionary<(char, Position), Position> Reflections = new()
        {
            { ('/', East), North },
            { ('/', West), South },
            { ('/', North), East },
            { ('/', South), West },
            { ('\\', East), South },
            { ('\\', West), North },
            { ('\\', North), West },
            { ('\\', South), East },
        };

        public override object Solve1()
        {
            var beam = (location: new Position(-1, 0), direction: East);
            Map = DataLines;

            RunBeams(new[] { beam });
            return EnergisedTiles.Count;
        }

        public override object Solve2()
        {
            Dictionary<Position, int> values = new();

            for (int i = 0; i < Map.Length; i++)
            {
                // from left
                var a = new Beam { location = new Position(-1, i), direction = East };
                values.Add((-1, i), RunPart2(a));

                // from top
                var b = new Beam { location = new Position(i, -1), direction = South };
                values.Add((i, -1), RunPart2(b));

                var c = new Beam { location = new Position(i, Map.Length), direction = North };
                values.Add((i, Map.Length), RunPart2(c));

                var d = new Beam { location = new Position(Map.Length, i), direction = West };
                values.Add((Map.Length, i), RunPart2(d));
            }

            return values.Max(x => x.Value);
        }

        public int RunPart2(Beam beam)
        {
            EnergisedTiles.Clear();
            Evaluated.Clear();

            RunBeams(new[] { beam } );
            return EnergisedTiles.Count;
        }

        public List<(Position location, Position direction)> Beams = new();
        public HashSet<Position> EnergisedTiles = new();
        public HashSet<(Position, Position)> Evaluated = new HashSet<(Position, Position)>();
        public string[] Map;

        public void RunBeams(IEnumerable<Beam> beams)
        {
            var queue = new Queue<(Position, Position)>(beams);

            while(queue.TryDequeue(out var beam))
            {
                
                if (!Evaluated.Add(beam))
                    continue;
                
                var newBeams = BeamTick(beam).ToList();
                foreach (var b in newBeams)
                {
                    EnergisedTiles.Add(b.location);
                    queue.Enqueue(b);
                }
            }
        }

        public IEnumerable<(Position location, Position direction)> BeamTick((Position location, Position direction) beam)
        {
            // move beam in the given direction.
            beam.location += beam.direction;
            var pos = beam.location;
            
            if (pos.X < 0 || pos.Y < 0 || pos.X >= Map.Length || pos.Y >= Map.Length)
            {
                yield break;
            }

            char c;

            // check the new location for interactions
            try
            {
                c = Map[beam.location.Y][beam.location.X];
            }
            catch (IndexOutOfRangeException)
            {
                yield break;
            }
            if (c == '\\' || c == '/')
            {
                beam.direction = Reflections[(c, beam.direction)];
                yield return beam;
            }
            else if (c == '|' && (beam.direction == East || beam.direction == West))
            {
                yield return new Beam { location = beam.location, direction = North };
                yield return new Beam { location = beam.location, direction = South };
            }
            else if (c == '-' && (beam.direction == North || beam.direction == South))
            {
                yield return new Beam { location = beam.location, direction = East };
                yield return new Beam { location = beam.location, direction = West };
            }
            else
            {
                yield return beam;
            }
        }
    }
}
