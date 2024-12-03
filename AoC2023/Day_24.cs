namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class Day_24 : BaseDay
    {
        public record struct Hailstone3d
        {
            public static Hailstone3d ParseString(string str)
            {
                var splits = str.Split("@");
                var h = new Hailstone3d();

                h.Position = new Position3dLong(splits[0].GetSignedLongs());
                h.Velocity = new Position3dLong(splits[1].GetSignedLongs());

                return h;
            }

            public Position3dLong Position;
            public Position3dLong Velocity;
        }
        public record struct Hailstone2d
        {
            public static Hailstone2d ParseString(string str)
            {
                var splits = str.Split("@");
                var h = new Hailstone2d();

                h.Position = new PositionDouble(splits[0].GetSignedLongs().Select(x => (double)x));
                h.Velocity = new PositionDouble(splits[1].GetSignedLongs().Select(x => (double)x));

                return h;
            }

            public PositionDouble Position;
            public PositionDouble Velocity;
        }
        


        #region Part1
        public override object Solve1()
        {
            var range = new IntervalDouble(200000000000000L, 400000000000000L + 1);
            var hailstones = DataLines.Select(Hailstone2d.ParseString).ToList();

            var collisionCount = 0;

            for (int i = 0; i < hailstones.Count; i++)
            {
                for (int j = i + 1; j < hailstones.Count; j++)
                {
                    var h1 = hailstones[i];
                    var h2 = hailstones[j];

                    var coll = Intersection(h1, h2);

                    if (coll == null)
                        continue;

                    var collValue = coll.Value;
                    if (range.Contains(collValue.X) && range.Contains(collValue.Y))
                    {
                        if (collValue.X > h1.Position.X == h1.Velocity.X > 0 
                            && collValue.Y > h1.Position.Y == h1.Velocity.Y > 0 
                            && collValue.X > h2.Position.X == h2.Velocity.X > 0
                            && collValue.Y > h2.Position.Y == h2.Velocity.Y > 0)
                            collisionCount++;
                    }
                }
            }

            return collisionCount;
        }

        public PositionDouble? Intersection(Hailstone2d h1, Hailstone2d h2)
        {
            var x1 = h1.Position.X;
            var x2 = h1.Position.X + h1.Velocity.X;
            var x3 = h2.Position.X;
            var x4 = h2.Position.X + h2.Velocity.X;

            var y1 = h1.Position.Y;
            var y2 = h1.Position.Y + h1.Velocity.Y;
            var y3 = h2.Position.Y;
            var y4 = h2.Position.Y + h2.Velocity.Y;

            var denom = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
            if (denom == 0) return null;
            var ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / denom;
            return new PositionDouble(
                x1 + ua * (x2 - x1),
                y1 + ua * (y2 - y1)
            );



            //var h1x0 = h1.Position.X;
            //var h1x1 = h1.Position.X + h1.Velocity.X;

            //var h1y0 = h1.Position.Y;
            //var h1y1 = h1.Position.Y + h1.Velocity.Y;

            //var h2x0 = h2.Position.X;
            //var h2x1 = h2.Position.X + h2.Velocity.X;

            //var h2y0 = h2.Position.Y;
            //var h2y1 = h2.Position.Y + h2.Velocity.Y;


            //var denom = (h2.Velocity.Y) * (h1.Velocity.X) - (h2.Velocity.X * h1.Velocity.Y);
            //if (denom == 0) return null;

            //var ua = h2.Velocity.X * (h1y0 - h2y0) - (h2.Velocity.Y) * (h1x0 - h2x0) / (double)denom;
            ////var ub = h1.Velocity.X * (h1y0 - h2y0) - (h1.Velocity.Y) * (h1x0 - h2x0) / (double)denom;
            //return new PositionDouble(
            //    h1.Position.X + ua * h1.Velocity.X,
            //    h1.Position.Y + ua * h1.Velocity.Y
            //);
        }

        //public void CheckCollisions(Hailstone2d a, Hailstone2d b)
        //{
        //    var top = -(a.Position.X * a.Velocity.X
        //        - a.Velocity.X * b.Position.X
        //        - (a.Position.X - b.Position.X) * b.Velocity.X
        //        + a.Position.Y * a.Velocity.Y
        //        - a.Velocity.Y * b.Position.Y
        //        - (a.Position.Y - b.Position.Y) * b.Velocity.Y);

        //    var bottom = a.Velocity.X * a.Velocity.X
        //        - 2 * a.Velocity.X * b.Velocity.X
        //        + b.Velocity.X * b.Velocity.X
        //        + a.Velocity.Y * a.Velocity.Y
        //        - 2 * a.Velocity.Y * b.Velocity.Y
        //        + b.Velocity.Y * b.Velocity.Y;

        //    var time = top / (double)bottom;

        //    var minDist = Math.Sqrt(
        //            Math.Pow(time * a.Velocity.X - time * b.Velocity.X + a.Position.X - b.Position.X, 2)
        //            + Math.Pow(time * a.Velocity.Y - time * b.Velocity.Y + a.Position.Y - b.Position.Y, 2)
        //        );
        //}

        #endregion

        #region Part2
        public override object Solve2()
        {
            return "";
        }
        #endregion
    }
}
