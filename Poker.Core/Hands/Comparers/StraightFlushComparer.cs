using System;
using Catel;
using Poker.Core.Deck;

namespace Poker.Core.Hands.Comparers
{
    /// <summary>
    /// Five cards of the same suit with consecutive values. Ranked by the highest card in the hand.
    /// </summary>
    public class StraightFlushComparer : IHandComparer
    {
        #region Implementation of IHandComparer

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

            var suit = hand[0].Suit;
            for (int i = 0; i < hand.Count-1; i++)
            {
                // Make sure the suits all match.
                if (suit != hand[i+1].Suit)
                    return Tuple.Create<Hands, Rank>(0, 0);

                // Check that the ranks are sequential.
                if (hand[i + 1].Rank != hand[i].Rank - 1)
                    return Tuple.Create<Hands, Rank>(0, 0);
            }

            // sequential and suits match, straight flush
            return Tuple.Create(Hands.StraightFlush, hand[0].Rank);
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

            if (x.Type != Hands.StraightFlush &&
                y.Type != Hands.StraightFlush)
                throw new InvalidOperationException("Cannot compare hands if neither hand has a Straight Flush.");

            if (x.Type != Hands.StraightFlush &&
                y.Type == Hands.StraightFlush)
                return -1 * Constants.SortDirection;

            if (x.Type == Hands.StraightFlush &&
                y.Type != Hands.StraightFlush)
                return 1 * Constants.SortDirection;

            if (x.HighCard < y.HighCard)
                return -1 * Constants.SortDirection;
            if (x.HighCard > y.HighCard)
                return 1 * Constants.SortDirection;

            return 0;
        }

        #endregion
    }
}