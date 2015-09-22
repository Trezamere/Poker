using System.Collections.Generic;
using Catel;
using Catel.IoC;

namespace Poker.Core
{
    /// <summary>
    /// Comparer which handles comparing the hands of two players.
    /// </summary>
    [ServiceLocatorRegistration(typeof(IComparer<IPlayer>))]
    public class PlayerHandComparer : IComparer<IPlayer>
    {
        #region Implementation of IComparer<in Player>

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param>
        public int Compare(IPlayer x, IPlayer y)
        {
            Argument.IsNotNull(() => x);
            Argument.IsNotNull(() => x.Hand);
            Argument.IsNotNull(() => y);
            Argument.IsNotNull(() => y.Hand);

            return x.Hand.CompareTo(y.Hand);
        }

        #endregion
    }
}