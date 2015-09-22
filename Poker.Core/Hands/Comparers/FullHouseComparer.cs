using System;
using System.Collections.Generic;
using Catel;
using Poker.Core.Deck;

namespace Poker.Core.Hands.Comparers
{
    /// <summary>
    /// Three cards of the same value, with the remaining two cards forming a pair. Ranked by the value of the three cards.
    /// </summary>
    public class FullHouseComparer : IHandComparer
    {
        /// <summary>
        /// The comparer used to compare high card values.
        /// </summary>
        private readonly IHandComparer _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FullHouseComparer"/> class.
        /// </summary>
        /// <param name="comparer"></param>
        public FullHouseComparer(IHandComparer comparer)
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

            // If count does not have two elements, then we have more than 2 kinds of cards in the hand, can't be full house.
            if (map.Count != 2)
                return Tuple.Create<Hands, Rank>(0, 0);

            // Get the entry with 3 
            foreach (var kvp in map)
            {
                if (kvp.Value == 3)
                    return Tuple.Create(Hands.FullHouse, kvp.Key);
            }

            // Should never get here, but compiler.
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

            if (x.Type != Hands.FullHouse &&
                y.Type != Hands.FullHouse)
                throw new InvalidOperationException("Cannot compare hands if neither hand is a FullHouse");

            if (x.Type != Hands.FullHouse &&
                y.Type == Hands.FullHouse)
                return -1 * Constants.SortDirection;

            if (x.Type == Hands.FullHouse &&
                y.Type != Hands.FullHouse)
                return 1 * Constants.SortDirection;

            // HighCard is 3 of a kind
            if (x.HighCard < y.HighCard)
                return -1 * Constants.SortDirection;
            if (x.HighCard > y.HighCard)
                return 1 * Constants.SortDirection;

            // High cards equal, just compare the rest of the cards.
            return _comparer.Compare(x, y);
        }

        #endregion
    }
}