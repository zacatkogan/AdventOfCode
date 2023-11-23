using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils
{
    public static class VectorHelpers
    {
        /// <summary>
        /// A list of all the possible 3d integer rotations
        /// </summary>
        public static List<int[,]> RotationVectors3d = new List<int[,]>
        {
            new int[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } },
            new int[,] { { 0, -1, 0 }, { 1, 0, 0 }, { 0, 0, 1 } },
            new int[,] { { -1, 0, 0 }, { 0, -1, 0 }, { 0, 0, 1 } },
            new int[,] { { 0, 1, 0 }, { -1, 0, 0 }, { 0, 0, 1 } },
            new int[,] { { 0, 0, 1 }, { 0, 1, 0 }, { -1, 0, 0 } },
            new int[,] { { 0, 0, 1 }, { 1, 0, 0 }, { 0, 1, 0 } },
            new int[,] { { 0, 0, 1 }, { 0, -1, 0 }, { 1, 0, 0 } },
            new int[,] { { 0, 0, 1 }, { -1, 0, 0 }, { 0, -1, 0 } },
            new int[,] { { -1, 0, 0 }, { 0, 1, 0 }, { 0, 0, -1 } },
            new int[,] { { 0, 1, 0 }, { 1, 0, 0 }, { 0, 0, -1 } },
            new int[,] { { 1, 0, 0 }, { 0, -1, 0 }, { 0, 0, -1 } },
            new int[,] { { 0, -1, 0 }, { -1, 0, 0 }, { 0, 0, -1 } },
            new int[,] { { 0, 0, -1 }, { 0, 1, 0 }, { 1, 0, 0 } },
            new int[,] { { 0, 0, -1 }, { 1, 0, 0 }, { 0, -1, 0 } },
            new int[,] { { 0, 0, -1 }, { 0, -1, 0 }, { -1, 0, 0 } },
            new int[,] { { 0, 0, -1 }, { -1, 0, 0 }, { 0, 1, 0 } },
            new int[,] { { 1, 0, 0 }, { 0, 0, -1 }, { 0, 1, 0 } },
            new int[,] { { 0, -1, 0 }, { 0, 0, -1 }, { 1, 0, 0 } },
            new int[,] { { -1, 0, 0 }, { 0, 0, -1 }, { 0, -1, 0 } },
            new int[,] { { 0, 1, 0 }, { 0, 0, -1 }, { -1, 0, 0 } },
            new int[,] { { -1, 0, 0 }, { 0, 0, 1 }, { 0, 1, 0 } },
            new int[,] { { 0, 1, 0 }, { 0, 0, 1 }, { 1, 0, 0 } },
            new int[,] { { 1, 0, 0 }, { 0, 0, 1 }, { 0, -1, 0 } },
            new int[,] { { 0, -1, 0 }, { 0, 0, 1 }, { -1, 0, 0 } },
        };

        public static int[] Rotate3d(int[,] rotationMatrix3d, int[] vector3d)
        {
            var result = new int[3];

            for (int i = 0; i < 3; i++)
            for (int k = 0; k < 3; k++)
            {
                result[i] += rotationMatrix3d[i, k] * vector3d[k];
            }

            return result;
        }

        public static Position3d Rotate3d(int[,] rotationMatrix3d, Position3d vector)
        {
            var result = Rotate3d(rotationMatrix3d, vector.ToArray());
            return (result[0], result[1], result[2]);
        }

        public static Position3d Rotate3d(this Position3d vector, int[,] rotationMatrix3d)
        {
            var result = Rotate3d(rotationMatrix3d, vector.ToArray());
            return (result[0], result[1], result[2]);
        }

        public static IEnumerable<Position3d> Rotate3d(this IEnumerable<Position3d> vectors, int [,] rotationMatrix3d)
        {
            return vectors.Select(x => Rotate3d(x, rotationMatrix3d));
        }


    }
}
