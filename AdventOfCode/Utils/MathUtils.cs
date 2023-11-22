using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils
{
    internal class MathUtils
    {
        public IEnumerable<int> GetFactors(int input)
        {
            yield return 1;

            for (int i = 2; i < input; i++)
            {
                var test = input / i;
                if (test * i == input)
                    yield return i;
            }

            yield break;
        }

    }
}
