using System;
using System.Collections.Generic;
using Catel;
using Poker.Core.Deck;

namespace Poker.Core.Hands.Comparers
{
    /// <summary>
    /// The hand contains two different pairs. Hands which both contain two pairs are ranked by the value of their highest pair. 
    /// Hands with the same highest pair are ranked by the value of their other pair. If these values are the same the hands are ranked by the value of the remaining card.
    /// </summary>
    public class TwoPairComparer : IHandComparer
    {
        #region Implementation of IComparer<in Hand>

        /// <summary>
        /// Compares the specified hand to determine if it is of the same type as this comparer.
        /// </summary>
        /// <param name="hand">The hand to be compared.</param>
        /// <returns>
        /// a tuple specifying information about the comparison.
        /// <para>The first value returns the type of this comparer if the specified hand matches, otherwise it returns <see cref="Hands.HighCard"/>.</para>
        /// <para>The second value indicates the high card of the hand if there was a match, otherwise it returns <see cref="Rank.Two"/>.</para>
        /// </returns>
        public Tuple<Hands, Rank> Compare(IHand hand)
        {
            Argument.IsNotNull(() => hand);
            if (hand.Count < 5)
                return Tuple.Create<Hands, Rank>(0, 0);

            // create a map of the rank and count for each card in the hand.
            var map = new Dictionary<Rank, int>(hand.Count);
            for (int i = 0; i < hand.Count; i++)
            {
                if (map.ContainsKey(hand[i].Rank))
                    map[hand[i].Rank]++;
                else
                    map[hand[i].Rank] = 1;
            }

            // If the map has more than three entries, then we have more than 3 kinds of cards in the hand, can't be two pair
            if (map.Count > 3)
                return Tuple.Create<Hands, Rank>(0, 0);

            Rank high = Rank.Two;
            foreach (var kvp in map)
            {
                // Grab the highest pair.
                if (kvp.Value == 2 && high < kvp.Key)
                    high = kvp.Key;
            }

            return Tuple.Create(Hands.TwoPair, high);
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param>
        public int Compare(IHand x, IHand y)
        {
            Argument.IsNotNull(() => x);
            Argument.IsNotNull(() => y);
            if (x.Count != y.Count)
                throw new InvalidOperationException("Cannot compare hands with a different number of cards");

            if (x.Type != Hands.TwoPair &&
                y.Type != Hands.TwoPair)
                throw new InvalidOperationException("Cannot compare hands if neither hand has Two Pair.");

            if (x.Type != Hands.TwoPair &&
                y.Type == Hands.TwoPair)
                return -1 * Constants.SortDirection;

            if (x.Type == Hands.TwoPair &&
                y.Type != Hands.TwoPair)
                return 1 * Constants.SortDirection;

            if (x.HighCard < y.HighCard)
                return -1 * Constants.SortDirection;
            if (x.HighCard > y.HighCard)
                return 1 * Constants.SortDirection;
            
            // create a map of the rank and count for each card in the hand.
            var xMap = new Dictionary<Rank, int>(3);
            var yMap = new Dictionary<Rank, int>(3);
            for (int i = 0; i < x.Count; i++)
            {
                if (xMap.ContainsKey(x[i].Rank))
                    xMap[x[i].Rank]++;
                else
                    xMap[x[i].Rank] = 1;

                if (yMap.ContainsKey(y[i].Rank))
                    yMap[y[i].Rank]++;
                else
                    yMap[y[i].Rank] = 1;
            }

            // Compare the second pair.
            Rank xPair = x.HighCard;
            Rank yPair = y.HighCard;

            Rank xSingle = Rank.Two;
            Rank ySingle = Rank.Two;

            foreach (var kvp in xMap)
            {
                // Get x second pair high card.
                if (kvp.Key != x.HighCard &&
                    kvp.Value == 2)
                    xPair = kvp.Key;

                // Get the x high single card.
                if (kvp.Value == 1)
                    xSingle = kvp.Key;
            }

            foreach (var kvp in yMap)
            {
                // Get y second pair high card.
                if (kvp.Key != y.HighCard &&
                    kvp.Value == 2)
                    yPair = kvp.Key;

                // Get the y high single card.
                if (kvp.Value == 1)
                    ySingle = kvp.Key;
            }

            if (xPair < yPair)
                return -1 * Constants.SortDirection;
            if (xPair > yPair)
                return 1 * Constants.SortDirection;

            // Pairs are both equal, compare the single card...
            if (xSingle < ySingle)
                return -1 * Constants.SortDirection;
            if (xSingle > ySingle)
                return 1 * Constants.SortDirection;

            return 0;
        }

        #endregion
    }
}