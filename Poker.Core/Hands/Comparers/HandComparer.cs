using System;
using System.Collections.Generic;
using Catel;
using Poker.Core.Deck;

namespace Poker.Core.Hands.Comparers
{
    /// <summary>
    /// Handles comparing hands together and selecting the best possible hand.
    /// </summary>
    public class HandComparer : IHandComparer
    {
        /// <summary>
        /// The collection of comparers used to iterate through a hand and get the best possible hand.
        /// <para>These are kept in order of best-to-worst hand for comparison purposes.</para>
        /// </summary>
        private readonly List<IHandComparer> _comparers;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandComparer"/> class.
        /// </summary>
        public HandComparer(
            IHandComparer highCardComparer,
            IHandComparer pairComparer,
            IHandComparer twoPairComparer,
            IHandComparer threeOfAKindComparer,
            IHandComparer straightComparer,
            IHandComparer flushComparer,
            IHandComparer fullHouseComparer,
            IHandComparer fourOfAKindComparer,
            IHandComparer straightFlushComparer)
        {
            _comparers = new List<IHandComparer>
                {
                    straightFlushComparer,
                    fourOfAKindComparer,
                    fullHouseComparer,
                    flushComparer,
                    straightComparer,
                    threeOfAKindComparer,
                    twoPairComparer,
                    pairComparer,
                    highCardComparer
                };
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
            var comparison = Tuple.Create<Hands, Rank>(0, 0);
            foreach (var comparer in _comparers)
            {
                comparison = comparer.Compare(hand);

                // Item1 matching something other than HighCard indicates a match, so return.
                // In the case of an actual HighCard match, we will have set the value and HighCard and be done iterating.
                if (comparison.Item1 != 0)
                    return comparison;
            }

            return comparison;
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

            if (x.Type < y.Type)
                return -1 * Constants.SortDirection;
            if (x.Type > y.Type)
                return 1 * Constants.SortDirection;

            // Types are equal, compare
            // TODO: just select the comparer we want.
            foreach (var comparer in _comparers)
            {
                try
                {
                    return comparer.Compare(x, y);
                }
                catch (InvalidOperationException)
                {
                    // Move on to the next comparer.
                }
            }

            // HighCard comparer should catch-all but...
            throw new InvalidOperationException("There was a problem comparing the hands, could not determine a comparison value.");
        }

        #endregion
    }
}