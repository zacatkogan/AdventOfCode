namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;
    using System.Security.Cryptography.X509Certificates;
    using System.Xml.Linq;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class Day_25 : BaseDay
    {
        public class Node
        {
            public Node(string name)
            {
                Name = name;
            }

            public string Name;
            public List<string> Connections = new();
        }

        public record struct Link
        {
            public Link(string node1, string node2)
            {
                var comparer = StringComparer.Ordinal;

                if (comparer.Compare(node1, node2) < 0)
                {
                    Node1 = node1;
                    Node2 = node2;
                }
                else
                {
                    Node2 = node1;
                    Node1 = node2;
                }
            }

            public string Node1;
            public string Node2;
        }

        #region Part1
        public override object Solve1()
        {
            var nodes = new Dictionary<string, Node>();
            var links = new HashSet<Link>();
            
            foreach (var line in DataLines)
            {
                var splits = line.Split(':');
                var nodeName = splits[0];
                var connections = splits[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var c in connections)
                {
                    links.Add(new Link(nodeName, c));
                }
            }

            foreach (var l in links)
            {
                var name1 = l.Node1;
                var name2 = l.Node2;

                if (!nodes.ContainsKey(name1))
                {
                    nodes.Add(name1, new Node(name1));
                }
                if (!nodes.ContainsKey(name2))
                {
                    nodes.Add(name2, new Node(name2));
                }

                nodes[name1].Connections.Add(name2);
                nodes[name2].Connections.Add(name1);
            }

            var result = ConnectNodes(nodes, links);
            return result.Item1 * result.Item2;
        }

        public (int, int) ConnectNodes(Dictionary<string, Node> nodes, HashSet<Link> links)
        {
            var group1 = new Dictionary<string, Node>();
            var evaluatedConnections = new HashSet<Link>();
            var unevaluatedConnections = new PriorityQueue<Link, int>();
            var ignoredConnections = new HashSet<Link>();
            
            // pick a node at random
            var initialNode = nodes.Values.OrderByDescending(x => x.Connections.Count).First();
            group1.Add(initialNode.Name, initialNode);

            foreach (var c in initialNode.Connections)
                unevaluatedConnections.Enqueue(new Link(initialNode.Name, c), 0);

            var firstLink = initialNode.Connections.OrderBy(x => x).First();
            var nextNode = nodes[firstLink];
            group1.Add(nextNode.Name, nextNode);
            evaluatedConnections.Add(new Link(initialNode.Name, nextNode.Name));

            var secondLink = initialNode.Connections.OrderBy(x => x).Skip(1).First();
            var secondNode = nodes[secondLink];
            group1.Add(secondNode.Name, secondNode);
            evaluatedConnections.Add(new Link(initialNode.Name, secondNode.Name));

            foreach (var c in nextNode.Connections)
                unevaluatedConnections.Enqueue(new Link(nextNode.Name, c), 0);

            var limit = 5;

            while (unevaluatedConnections.TryDequeue(out var link, out int attempts))
            {
                if (evaluatedConnections.Contains(link))
                    continue;

                if (attempts >= limit && unevaluatedConnections.Count < 5)
                {
                    ignoredConnections.Add(link);
                    continue;
                }

                if (group1.ContainsKey(link.Node1) && group1.ContainsKey(link.Node2))
                {
                    evaluatedConnections.Add(link);
                    continue;
                }

                var nodeNameToEvaluate = group1.ContainsKey(link.Node1) ? link.Node2 : link.Node1;
                var nodeToEvaluate = nodes[nodeNameToEvaluate];

                // check that the new node shares a connection with at least 2 existing nodes
                if (nodeToEvaluate.Connections.Where(group1.ContainsKey).Count() >= 2)
                {
                    group1.Add(nodeNameToEvaluate, nodeToEvaluate);
                    evaluatedConnections.Add(link);
                    unevaluatedConnections.EnqueueRange(
                        nodeToEvaluate.Connections.Select(x => new Link(x, nodeNameToEvaluate)),
                        0
                    );
                }
                else if (attempts > limit && unevaluatedConnections.Count > 5)
                {
                    limit += 1;
                    group1.Add(nodeNameToEvaluate, nodeToEvaluate);
                    evaluatedConnections.Add(link);
                    unevaluatedConnections.EnqueueRange(
                        nodeToEvaluate.Connections.Select(x => new Link(x, nodeNameToEvaluate)),
                        0
                    );
                }
                else
                {
                    unevaluatedConnections.Enqueue(link, ++attempts);
                }
            }

            var group2 = nodes.Except(group1).ToDictionary();
            return (group1.Count, group2.Count);
        }
        #endregion

        #region Part2
        public override object Solve2()
        {
            return "Merry Christmas";
        }
        #endregion
    }
}
