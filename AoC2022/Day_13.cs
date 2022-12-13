using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace AdventOfCode
{
    public class Day_13 : BaseDay
    {
        public override object Solve1()
        {
            var pairsRaw = Data.Split("\n\n");
            var result = pairsRaw.Select(x => x.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                .ToList(x => (left:ParseListAsJson(x[0]), right:ParseListAsJson(x[1])))
                .ToList(x => CompareListsPatternMatching(x.left, x.right))
                .Select((x, i) => x < 0 ? i + 1: 0)
                .Sum();
            return result;
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
            var packet1 = ParseListAsJson("[[2]]");
            var packet2 = ParseListAsJson("[[6]]");

            var comparer = Comparer<JToken>.Create(CompareListsPatternMatching);

            var parsed = Data.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => ParseListAsJson(x))
                .Append(packet1)
                .Append(packet2)
                .ToList()
                .Order(comparer)
                .ToList();

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

        JToken ParseListAsJson(string str)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<JToken>(str);
        }

        bool? CompareLists(JToken left, JToken right)
        {
            for (int i = 0; i <= left.Count(); i++)
            {
                if (i == left.Count() && i == right.Count())
                    return null;

                if (i == left.Count() || i == right.Count())
                    return i == left.Count();

                var l = left[i];
                var r = right[i];

                if (l is JValue lint && r is JValue rint)
                {
                    if ((int)lint == (int)rint)
                        continue;
                    if (lint.Value<int>() < rint.Value<int>())
                        return true;
                    return false;
                }

                else if (l is JArray llist && r is JArray rlist)
                {
                    var result = CompareLists(llist, rlist);
                    if (result is null)
                        continue;
                    return result;
                }

                // otherwise one is int and one is list
                {
                    if (l is JValue)
                        l = new JArray(l);

                    if (r is JValue)
                        r = new JArray(r);

                    var result = CompareLists(l as JArray, r as JArray);
                        
                    if (result is null)
                        continue;
                    return result;
                }
            }

            return true;
        }

        // adapted to C# from https://github.com/jarshwah/advent-of-code/blob/main/python/2022/q13.py
        int CompareListsPatternMatching(JToken left, JToken right)
        {
            switch(left, right)
            {
                case (JValue l, JValue r):
                    return (int)l - (int)r;
                case (JArray l, JArray r):
                    return 
                    Enumerable.Zip(left, right)
                        .Select(x => CompareListsPatternMatching(x.First, x.Second))
                        .Where(x => x != 0)
                        .Append(left.Count() - right.Count())
                        .First();
                case (JArray l, JValue r):
                    return CompareListsPatternMatching(l, new JArray(r));
                case (JValue l, JArray r):
                    return CompareListsPatternMatching(new JArray(l), r);
                default:
                    throw new Exception();
            }
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
