using System;
using System.Collections.Generic;
using Catel;
using Poker.Core.Deck;

namespace Poker.Core.Hands.Comparers
{
    /// <summary>
    /// Two of the five cards in the hand have the same value. Hands which both contain a pair are ranked by the value of the cards forming the pair. 
    /// If these values are the same, the hands are ranked by the values of the cards not forming the pair, in decreasing order.
    /// </summary>
    public class PairComparer : IHandComparer
    {
        /// <summary>
        /// The comparer used to compare high card values.
        /// </summary>
        private readonly IHandComparer _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PairComparer"/> class.
        /// </summary>
        /// <param name="comparer"></param>
        public PairComparer(IHandComparer comparer)
        {
            _comparer = comparer;
        }

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

            // If the map has more than 4 entries, then we have more than 4 kinds of cards in the hand, can't be a pair
            if (map.Count > 4)
                return Tuple.Create<Hands, Rank>(0, 0);
            
            foreach (var kvp in map)
            {
                // Grab the pair.
                if (kvp.Value == 2)
                    return Tuple.Create(Hands.OnePair, kvp.Key);
            }

            // Shouldnt get here, but compiler.
            return Tuple.Create<Hands, Rank>(0, 0);
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

            if (x.Type != Hands.OnePair &&
                y.Type != Hands.OnePair)
                throw new InvalidOperationException("Cannot compare hands if neither hand has a Pair.");

            if (x.Type != Hands.OnePair &&
                y.Type == Hands.OnePair)
                return -1 * Constants.SortDirection;

            if (x.Type == Hands.OnePair &&
                y.Type != Hands.OnePair)
                return 1 * Constants.SortDirection;

            if (x.HighCard < y.HighCard)
                return -1 * Constants.SortDirection;
            if (x.HighCard > y.HighCard)
                return 1 * Constants.SortDirection;

            // Return the result of comparing the value high cards.
            return _comparer.Compare(x, y);
        }

        #endregion
    }
}