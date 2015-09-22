using System;
using System.Collections.Generic;
using Catel;
using Poker.Core.Deck;

namespace Poker.Core.Hands.Comparers
{
    /// <summary>
    /// Hands which do not fit any higher category are ranked by the value of their highest card. 
    /// If the highest cards have the same value, the hands are ranked by the next highest, and so on.
    /// </summary>
    public class HighCardComparer : IHandComparer
    {
        /// <summary>
        /// Handles comparing the ranks of cards.
        /// </summary>
        private readonly IComparer<Card> _cardComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighCardComparer"/> class.
        /// </summary>
        /// <param name="cardComparer"></param>
        public HighCardComparer(IComparer<Card> cardComparer)
        {
            _cardComparer = cardComparer;
        }
        
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
            if (hand.Count == 0)
                return Tuple.Create<Hands, Rank>(0, 0);

            return Tuple.Create(Hands.HighCard, hand[0].Rank);
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

            // We don't care about the Hand type when making a high card comparison.
            for (int i = 0; i < x.Count; i++)
            {
                // Return the first set of values that are not equal.
                var value = _cardComparer.Compare(x[i], y[i]);
                if (value != 0)
                    return value;
            }

            // Checked all the cards, all equal.
            return 0;
        }

        #endregion
    }
}