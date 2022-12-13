namespace AdventOfCode
{
    public class Day_13 : BaseDay
    {
        public override object Solve1()
        {
            //ParseList("[[4,4],4,4,4]");

            var pairsRaw = Data.Split("\n\n");
            var pairs = pairsRaw.Select(x => x.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                .Select(x => (left:ParseList(x[0]), right:ParseList(x[1]))).ToList();

            var test = pairs[3];
            CompareLists(test.left, test.right);

            var comparedPairs = pairs.Select(x => CompareLists(x.left, x.right)).ToList();
            return comparedPairs.Select((x, i) => x.GetValueOrDefault() ? i + 1: 0).Sum();
        }

        bool? CompareLists(List<object> left, List<object> right)
        {
            for (int i = 0; i <= left.Count; i++)
            {
                if (i == left.Count && i == right.Count)
                    return null;

                if (i == left.Count || i == right.Count)
                    return i == left.Count;

                var l = left[i];
                var r = right[i];

                if (l is int lint && r is int rint)
                {
                    if (lint == rint)
                        continue;
                    if (lint < rint)
                        return true;
                    return false;
                }

                else if (l is List<object> llist && r is List<object> rlist)
                {
                    var result = CompareLists(llist, rlist);
                    if (result is null)
                        continue;
                    return result;
                }

                // otherwise one is int and one is list
                {
                    if (l is int)
                        l = new List<object>{ l };

                    if (r is int)
                        r = new List<object> { r };

                    var result = CompareLists(l as List<object>, r as List<object>);
                        
                    if (result is null)
                        continue;
                    return result;
                }
            }

            return true;
        }
        
        int ListComparerFunc(List<object> left, List<object> right)
        {
            var result = CompareLists(left, right);
            if (result == null)
                return 0;
            return result.Value ? -1 : 1;
        }

        public override object Solve2()
        {
            var packet1 = ParseList("[[2]]");
            var packet2 = ParseList("[[6]]");

            var parsed = Data.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => ParseList(x)).ToList();
            parsed.Add(packet1);
            parsed.Add(packet2);

            var comparer = Comparer<List<object>>.Create(ListComparerFunc);
            parsed.Sort(comparer);

            var p1 = parsed.IndexOf(packet1) + 1;
            var p2 = parsed.IndexOf(packet2) + 1;

            return p1 * p2;
        }

        List<object> ParseList(string str)
        {
            var root = new List<object>();

            var currentList = root;
            var queue = new Stack<List<object>>();
            queue.Push(root);

            var intString = "";
            foreach (var c in str.Skip(1))
            {
                if (c == '[')
                {
                    var l = new List<object>();
                    queue.Peek().Add(l);
                    queue.Push(l);
                }
                else if (c == ',')
                {
                    if (!string.IsNullOrWhiteSpace(intString))
                        queue.Peek().Add(int.Parse(intString));
                    intString = "";
                }
                else if (c == ']')
                {
                    // close of list
                    if (!string.IsNullOrWhiteSpace(intString))
                        queue.Peek().Add(int.Parse(intString));
                    intString = "";
                    queue.Pop();
                }
                else
                {
                    intString += c;
                }
            }

            return root;
        }

        string testData = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]";
    }
}
