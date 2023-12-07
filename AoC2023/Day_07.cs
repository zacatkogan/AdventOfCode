namespace AdventOfCode.AoC2023
{
    using AdventOfCode;
    using AdventOfCode.Utils;
    using MathNet.Numerics;
    using MathNet.Numerics.RootFinding;
    using Spectre.Console;
    using System.Numerics;
    using System.Reflection.Emit;
    using System.Reflection.Metadata;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;

    public class Day_07 : BaseDay
    {
        string[] TestDataLines = "32T3K 765\nT55J5 684\nKK677 28\nKTJJT 220\nQQQJA 483".Split("\n");
        Dictionary<char, char> CardToValue = new Dictionary<char, char>()
        {
            { 'T', 'B' },
            { 'J', 'C' },
            { 'Q', 'D' },
            { 'K', 'E' },
            { 'A', 'F' },
        };

        Dictionary<char, char> CardToValueJokers = new Dictionary<char, char>()
        {
            { 'J', '1' },
            { 'T', 'B' },
            { 'Q', 'C' },
            { 'K', 'D' },
            { 'A', 'E' },
        };

        public override object Solve1()
        {
            var handRanksAndAnte = DataLines
                .Select(x => x.Split(" "))
                .Select(x => new { hand = x[0], handType = GetHandType(x[0]), ante = x[1].GetInts()[0] })
                .OrderBy(x => x.handType).ThenBy(x => RemapHandToValues(x.hand, CardToValue))
                .ToList();

            var winnings = 0;

            for (int i = 0; i < handRanksAndAnte.Count; i++)
            {
                winnings += handRanksAndAnte[i].ante * (i + 1);
            }

            return winnings;
        }

        public override object Solve2()
        {
            var handRanksAndAnte = DataLines
                .Select(x => x.Split(" "))
                .Select(x => new { hand = x[0], handType = GetHandTypeWithJokers(x[0]), ante = x[1].GetInts()[0] })
                .OrderBy(x => x.handType).ThenBy(x => RemapHandToValues(x.hand, CardToValueJokers))
                .ToList();

            int winnings = 0;

            winnings = handRanksAndAnte.Select((x, i) => x.ante * (i + 1)).Sum();

            return winnings;
        }

        public HandType GetHandType(IEnumerable<char> cards)
        {
            var groupedCards = cards.GroupBy(x => x) // group chars together
                .Select(x => x.Count())              // get num of chars in each group
                .OrderByDescending(x => x)           // order by group size
                .ToList();

            if (groupedCards[0] == 5)
                return HandType.FiveOfAKind;
            if (groupedCards[0] == 4)
                return HandType.FourOfAKind;
            if (groupedCards[0] == 3 && groupedCards[1] == 2)
                return HandType.FullHouse;
            if (groupedCards[0] == 3)
                return HandType.ThreeOfAKind;
            if (groupedCards[0] == 2 && groupedCards[1] == 2)
                return HandType.TwoPair;
            if (groupedCards[0] == 2)
                return HandType.OnePair;

            return HandType.HighCard;
        }

        public HandType GetHandTypeWithJokers(string cards)
        {
            if (!cards.Contains('J'))
                return GetHandType(cards);

            HandType bestHand = HandType.OnePair;

            foreach (var c in cards.Distinct())
            {
                var newHand = cards.Replace('J', c);
                var newHandType = GetHandType(newHand);
                bestHand = (HandType)Math.Max((int)bestHand, (int)newHandType);
            }

            return bestHand;
        }

        public enum HandType
        {
            FiveOfAKind = 6,
            FourOfAKind = 5,
            FullHouse = 4,
            ThreeOfAKind = 3,
            TwoPair = 2,
            OnePair = 1,
            HighCard = 0,
        }

        public string RemapHandToValues(string hand, Dictionary<char, char> mapping)
        {
            var h = hand;
            foreach (var m in mapping)
            {
                h = h.Replace(m.Key, m.Value);
            }

            return h;
        }
    }
}
