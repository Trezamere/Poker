using System;
using Catel.MVVM.Converters;
using Poker.Core;

namespace Poker.Application.Resources.Converters
{
    /// <summary>
    /// Outputs a formatted string of the player and their hand.
    /// </summary>
    public class PlayerToResultConverter : ValueConverterBase<IPlayer, string>
    {
        #region Overrides of ValueConverterBase<IPlayer,string>

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param><param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param><param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        protected override object Convert(IPlayer value, Type targetType, object parameter)
        {
            return $"{value.Name} with a {Extensions.GetEnumDescription(value.Hand.Type)}, {value.Hand.HighCard}-high.";
        }

        #endregion
    }
}