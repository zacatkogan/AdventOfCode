namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;
    using static AdventOfCode.AoC2023.Day_22;
    using MathNet.Spatial.Euclidean;
    using System.ComponentModel.DataAnnotations;

    public class Day_24 : BaseDay
    {
        public struct Hailstone
        {
            
            public Point3D Position;
            public Vector3D Velocity;

            public Line3D Line => new Line3D(Position, Position + Velocity);

            public static Hailstone Parse(string s)
            {
                var hs = new Hailstone();
                var splits = s.Split("@");
                
                var p = splits[0].GetSignedLongs().ToList();
                var v = splits[1].GetSignedLongs().ToList();

                hs.Position = new Point3D(p[0], p[1], p[2]);
                hs.Velocity = new Vector3D(v[0], v[1], v[2]);

                return hs;
            }
        }

        #region Part1
        public override object Solve1()
        {
            return "";
        }
        #endregion

        #region Part2
        public override object Solve2()
        {
            var hailstones = DataLines.Select(Hailstone.Parse).ToList();

            var pseudoParallelLines = new List<(Hailstone, Hailstone)>();
            // reduce it down to 2d (x,y) and find pairs of parallel lines
            // once extended to 3d, these pseudo-parallel lines will form a plane
            for (int i = 0; i < hailstones.Count; i++)
            {
                for (int j = i + 1; j < hailstones.Count; j++)
                {
                    var h1 = hailstones[i];
                    var h2 = hailstones[j];
                    var h1v = h1.Velocity;
                    var h2v = h2.Velocity;
                    var h1v_2d = new Vector2D(h1v.X, h1v.Y);
                    var h2v_2d = new Vector2D(h2v.X, h2v.Y);

                    if (h1v_2d.IsParallelTo(h2v_2d))
                        pseudoParallelLines.Add((h1, h2));
                }
            }

            List<Vector3D> pseudoParallelVectors = new();

            foreach (var (h1, h2) in  pseudoParallelLines)
            {
                var isParallel = h1.Line.IsParallelTo(h2.Line);
                var (p1, p2) = h1.Line.ClosestPointsBetween(h2.Line);
                var intersects = p1 == p2;
                if (!intersects)
                {
                    var newLine = new Line3D(p1, p2);
                    var newVector = new Vector3D(p1.X - p2.X, p1.Y -  p2.Y, p1.Z - p2.Z);
                    var crossProduct = h2.Velocity.CrossProduct(h1.Velocity);
                    //var unitVector = newVector.Normalize();

                    var parallel = newVector.IsParallelTo(crossProduct);

                }
            }


            //var throwVector = h1.Velocity.CrossProduct(h2.Velocity);
            //var closestPoint = h1.Line.ClosestPointsBetween(h2.Line);

            //var throwLine = new Line3D(closestPoint.Item1, closestPoint.Item1 + throwVector);

            //var areEqual =
            //    (from hs in hailstones
            //     let cp = throwLine.ClosestPointsBetween(hs.Line)
            //     let p1 = cp.Item1
            //     let p2 = cp.Item2
            //     select (p1 == p2)).All(x => x);

            //// throw is on the vector of `throwVector` that passes through both of `closestPoint`
            
            return "";
        }

        #endregion
    }
}
