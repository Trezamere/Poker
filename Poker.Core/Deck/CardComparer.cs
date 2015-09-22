using System.Collections.Generic;

namespace Poker.Core.Deck
{
    /// <summary>
    /// Compares two <see cref="Card"/> objects by <see cref="Rank"/>.
    /// </summary>
    public sealed class CardComparer : IComparer<Card>
    {
        #region Implementation of IComparer<in Card>

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param>
        public int Compare(Card x, Card y)
        {
            if (x.Rank < y.Rank)
                return -1 * Constants.SortDirection;

            if (x.Rank > y.Rank)
                return 1 * Constants.SortDirection;

            return 0;
        }

        #endregion
    }
}