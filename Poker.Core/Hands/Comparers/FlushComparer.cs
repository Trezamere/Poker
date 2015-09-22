using System;
using System.Linq;
using Catel;
using Poker.Core.Deck;

namespace Poker.Core.Hands.Comparers
{
    /// <summary>
    /// Hand contains five cards of the same suit. Hands which are both flushes are ranked using the rules for High Card.
    /// </summary>
    public class FlushComparer : IHandComparer
    {
        /// <summary>
        /// The comparer used to compare high card values.
        /// </summary>
        private readonly IHandComparer _highCardComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlushComparer"/> class.
        /// </summary>
        /// <param name="highCardComparer"></param>
        public FlushComparer(IHandComparer highCardComparer)
        {
            _highCardComparer = highCardComparer;
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

            var suit = hand[0].Suit;
            if (hand.Cast<Card>().Any(card => suit != card.Suit))
            {
                // Found a different suit, not a flush.
                return Tuple.Create<Hands, Rank>(0, 0);
            }

            // High card will be the first card, since the hand is already sorted by rank.
            return Tuple.Create(Hands.Flush, hand[0].Rank);
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

            if (x.Type != Hands.Flush &&
                y.Type != Hands.Flush)
                throw new InvalidOperationException("Cannot compare hands if neither hand is a Flush");

            if (x.Type != Hands.Flush &&
                y.Type == Hands.Flush)
                return -1 * Constants.SortDirection;

            if (x.Type == Hands.Flush &&
                y.Type != Hands.Flush)
                return 1 * Constants.SortDirection;

            // Both flushes, rank via high card rules.
            return _highCardComparer.Compare(x, y);
        }

        #endregion
    }
}