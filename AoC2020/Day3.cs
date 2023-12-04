﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.AOC2020
{
    internal class Day3
    {
        public static void PartOne()
        {
            int collisions = 0;

            int x = 0, y = 0;
            int xIncrement = 1, yIncrement = 3;
            var map = GetMap();

            while(x < map.Length)
            {
                // check for collisions, wrapping around y
                var row = map[x];
                y %= row.Length;

                if (row[y] == '#')
                    collisions++;

                // increment position
                x += xIncrement;
                y += yIncrement;
            }

            Console.WriteLine(collisions);
        }

        public static void PartTwo()
        {
            var product = new List<(int y, int x)>()
            {
                (1, 1),
                (3,1),
                (5,1),
                (7,1),
                (1,2)
            }.Select(grad =>
            {
                var collisions = (long)GetCollisions(grad.x, grad.y);
                Console.WriteLine($"xinc: {grad.x}, yinc: {grad.y}, collisions: {collisions}");
                return collisions;
            })
            .Aggregate(1L, (a, b) => a * b);

            Console.WriteLine(product);
        }

        public static int GetCollisions(int xIncrement, int yIncrement)
        {
            int collisions = 0;

            int x = 0, y = 0;
            var map = GetMap();

            while (x < map.Length)
            {
                // check for collisions, wrapping around y
                var row = map[x];
                y %= row.Length;

                if (row[y] == '#')
                    collisions++;

                // increment position
                x += xIncrement;
                y += yIncrement;
            }

            return collisions;
        }



        public static char[][] GetMap()
        {
            return rawData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToArray()).ToArray();
        }

        public static string rawData = @".....##.#.....#........#....##.
....#...#...#.#.......#........
.....##.#......#.......#.......
...##.........#...#............
........#...#.......#.........#
..........#......#..#....#....#
..................#..#..#....##
.....##...#..#..#..#..#.##.....
..##.###....#.#.........#......
#.......#......#......#....##..
.....#..#.#.......#......#.....
............#............#.....
...#.#........#........#.#.##.#
.#..#...#.....#....##..........
##..........#...#...#..........
...........#...###...#.......##
.#..#............#........#....
##.#..#.....#.......#.#.#......
.##.....#....#.#.......#.##....
..##...........#.......#..##.#.
##...#.#........#..#...#...#..#
.#..#........#.#.......#..#...#
.##.##.##...#.#............##..
..#.#..###......#..#......#....
.#..#..#.##.#.##.#.#...........
...#....#..#.#.#.........#..#..
......#.#....##.##......#......
#....#.##.##....#..#...........
...#.#.#.#..#.#..#.#..#.##.....
#.....#######.###.##.#.#.#.....
..#.##.....##......#...#.......
..#....#..#...##.#..#..#..#..#.
.............#.##....#.........
.#....#.##.....#...............
.#............#....#...#.##....
.#.....#.##.###.......#..#.....
.#...#.........#.......#..#....
..#.#..#.##.......##...........
.....##..#..#..#..#.##..#.....#
..##............##...#..#......
...#..#....#..##.....##..#.#...
#.....##....#.#.#...#...#..##.#
#.#..#.........#.##.#...#.#.#..
.....#.#....##....#............
#.......#..#.....##..#...#...#.
.....#.#...#...#..#......#.....
..##....#.#.#.#.#..#...........
##..#...#.........#......#...#.
..#...#.#.#.#..#.#.##..##......
#............###.....###.......
..........#...#........###.....
.......##...#...#...#........#.
.#..#.##.#.....................
.#..##........##.##...#.......#
.......##......#.....#......#..
.##.#.....#......#......#......
#...##.#.#...#.#...............
........#..#...#.##.......#....
...................#...#...##..
...#...#.........#.....#..#.#..
.###..#........#..##.##..#.##..
#...#.....#.....#.....#..#..#..
###..#.....#.#.#.#......#....#.
#........#....##.#...##........
.#.#..##........##....##.#.#...
#...#....#.###.#.#.........#...
...#...##..###.......#.........
......#....#..##..#.....#.#....
........#...##...###......##...
..........##.#.......##........
...#....#......#...##.....#....
###.#.....#.#..#..#....#...#..#
.#.....#.#....#...............#
..#....#....####....###....#.#.
....##........#..#.##.#....#...
.......##...#...#..#....####...
#...##.#......##...#..#........
..##..#.##....#.......##.#.#...
..#.#...............#...#.#....
....#.....#.#.....#.##.......#.
...#.#..##.#.#..............##.
..#.....#...#.............#.##.
##..#.#...#........#..#.....##.
...........##...#.#.###...#....
...#.#.#..#..................#.
.#...##.............#...#......
..#..#...#.#.......#...#.....#.
..##.......#.#.................
.##..#........###.....#....#.##
......#..###.......#....##....#
....#.....#.................#..
........#...#...#..............
...#..#.###.......#..#.#.#.##..
..#...#.....#....#.........#...
...#.............#........###..
......#..............#......#..
#..#...........#...#..........#
...##...#.###..#...#.....#.#...
....#..##......#.......##......
....#....##.#...#.#..#....#...#
.#...........#..#....##...#..##
..#.#.................###.#...#
..#.#.#...##...........#.......
..........#..##...#.#..##....##
........#........#.##..#.#...#.
.....#...##.......##......#...#
....#...#..#..#.....#..........
.#..#......#..#..#..###.......#
.##..........#...#...#.#.....##
..#..........#.#.#...###.......
....#................#...##....
.##..#....#..........#.#.#.....
..##...#.#........#.....#.##...
....####.....#..#.........##..#
......#.........#...#..........
....#...................#..##..
.##....#.#.........#....#...#..
....##...##.....#..####........
..##.#....#.#.......##...#.....
#...#.#.#...#..#..##.....#.....
#..................###.....#...
#.#.....#.......#.#...###.#....
.#..#....#............#........
#.#....#..#.#...............#..
..#..#..#.............#......#.
..#.......##...................
.#....#.........#....#.#.#..#..
....#....#..#...............#..
......#..#..##......#.........#
..#.##........##......#..#..#.#
#.....#.#....#.........##...#..
###..............#....###...##.
....#..##......#.......##......
......#...#.##......##....#..#.
..........#....#..##.......#..#
.#..#...##..#...........#..#..#
.....#....#...#..###...###....#
.#####..#...#.#.#..#.#.###...##
..##............##.#...#.##...#
.##..#...#...#....##.#..#..##..
.#....#...#............##..#...
.#.#......#....#....#..##..##..
.........#...#.......#.##..#...
#.........#.....##.....#..#..#.
...##.#...#...#..#..#....##..##
.#............#...#....##......
..#...#.##.........#.#......#.#
....#.##........#.........#..##
#.........#......#.#......#..#.
........#.#.......#.#........#.
..#..........##.#...#..#.#.....
..#...#....#...#...#..#.#..#.#.
.#.........#....#..#####..#....
#.#....#.#.###...#.............
..##...........##......##......
#.....#..#....#...............#
...#.#..#....##......#...##....
...#........#.....#...#..#.....
.#......##.........#......#....
..#..###.##...#.#.....#........
.............#......#..#.......
..#...............#.#...#..#..#
.......#..#...#.#####......#..#
.........#.....#...............
##........#............#.#.....
.#...#.....#..#..#...#....#...#
..#....#....##......##.....#.#.
#...##..##......#...#....#.....
....#.#.#.....###....##.##....#
..........##...##.......#......
..#.......#...##.#....##.##....
....#........................#.
...#...#.#.##...#.....#...#..#.
.#....##..#..#..........##..##.
.#.....#..#...#.##.....#.......
.#.##...#.#..#.....##....#...#.
.##...#........##....#..#......
.....#........#..........#.#..#
....#..##.......#..#.....#.....
...........#...#........#.##..#
.....#..#....#..#.#.....#....##
.....#....#.##.#..##...........
...##.......##.........#.......
...............##..#....#.#....
.......###..#........#..####.##
.......#.##...#.#....#.####....
....#...............#..........
##.#.......#.....#......#...#..
......##.....#....#.....#..#..#
.....#...##.............#......
#.#.##.#.....#..#........#.....
......##....#..#........#......
............#........#..#.#....
##.......#......#...####..#.##.
..##..#...#.............#.##...
.....#..##......#.##......###..
............#........#........#
#.#.#.#...#.#.....#.........#..
.........#...............#.....
.............###.#.......#....#
###.##..#..#..........#....#...
#......#...#..#..#.....#.##....
............#....#....#..#.....
..#.#....#...#......#.#..#..##.
...#........................#..
#.#...#..........#......#.#....
.........#................#...#
##.....#....#........##.......#
#...##........#...#...........#
...#...#..........##.......#.#.
..#.#.#....#......##...........
...#.#...#.##.#..#.#.##........
#....##.....###..#.......#.....
###.....#.#.#...#..#.........##
..#......#..###...#.#.#.....#.#
.#....#.....#............#..##.
....#....##..........#.....##..
#...........#....#...#..#...##.
..#.......#.....#..........#...
.#..#................#......#..
..#......#.#...#..#.#....#....#
...#..#...###..#..##....#.#....
..#..............#.....#.......
...#.#...#.........#.#.........
##......##...........##.#.##..#
..#..##..#....#.#......#.#...##
...#.###....###...#.....#......
#.#................#......#....
..#.....#.....#....##.......#..
.#.#...............##..#.......
...#....#.......#.#.....##..#..
.........#....#.......#.#...##.
#....#......##.#.........##...#
#.............#..##.#.#..##....
...#....#..#...#....#.#.#.#...#
.#....#....#..##.....#.#...###.
##............#.#...##.#..#.#..
##.#....##.....#..#..###....#..
##....#................##......
...##..#...#..###....#.....##..
.#...##......#..#.#.....#...#..
..##......##...#.##.......#....
......#.....#.....##........#.#
##....#...........#............
#.......#....#..#.##..##.#..#..
.#....##.#.....#..#..#.........
.#....#.#.#...#.....##.....#.#.
.......##.#.#........#......##.
##........#.##.......#...#..#..
...###..##....#.#....#.#.......
......#.......#...##.....#...#.
..#......##.#......#.....#.....
.....#.....###...#.............
#...#.#...#...#..#......#......
#.....#.......###.#....###.#...
...#.......#....####....##..#..
#.#.....#....#........#.......#
.........#.......#......#.#...#
..##....#.....##...............
..........#..#.#..#......#.....
..................##...##.#....
........#.......#...#..#.#.#...
.....#.#..##..#..#.#..#.......#
.....#........#..#..#....#....#
##............#..#..#...#....#.
.....#....................##..#
........##.#....###............
##.......#.##................#.
.....###.#..#..#...#....###.##.
.#......#.#....#.....##.#......
...##......##.........#...#....
....####..............#........
#...#.#..##..##.........##.....
......#......#....#..#.........
#.....#.....#.##...............
..#.##..#...##.#.####..#....###
#..#......#....#.##..##...#.#..
#....#.......#.....#.....#.#...
##.......#.....##...#.....#....
...#...##..........#..##..##..#
.###..#..##...#....#...#..#....
......##..###.......###...#....
....#...#.#.......#.##...##..##
#.#......#..##.#.#..#..#..#....
......#........#.......#.......
..........#.#.....##...........
......#..#........#..#.#..###..
##..#.............##..#........
.........#....#.....#.........#
.....#..##...#..#..##.##......#
###..#...........#.......#....#
...............#....#.#........
.##.#...#.#........##....#.....
.##.###...##..###....#...#...#.
.##..#....#.#.#...#.#.#.#...#..
.###.#...#.......#....#..#.....
..#..#.#.#.#........#.....##...
.#.......#.#...#.#...........##
...#.....##....#.....##...#....
................#.....####...#.
.#.#......#.......##...#.##....
.###.........#.#......#..#.#...
#......#...#....#..##.......#..
.##..#....#..#...........#...#.
.#...#.......##........#.##....
..#...........#...##...........
.....##....##......#....#..#...
#......#.#...#.##.#...##....#..
#....................#...##...#
..#............#........#......
.............#.........##.....#
...#...#......##.#...#...#.#...
..#...#.#.................#....
....##...#....#...###.##......#
...#....#...#..#...#....#.....#
...##.#........#..#.........#..
..##.....#..##...#.....##...#..
#.........#.#.#...#......#...#.
#.#...........#...#..#..#..##..
..#..#..##....#..........#.###.
.....#..#....#.#...#...#..#..#.
###.....#..#.................#.
.#..##.##.#......#....##..#....";
    }
}