using System;
using System.Windows.Data;
using Catel.MVVM.Converters;
using Poker.Core.Deck;

namespace Poker.Application.Resources.Cards.Converters
{
    /// <summary>
    /// Converts a <see cref="Rank"/> to a friendly string representation.
    /// </summary>
    public class RankToFriendlyTextConverter : ValueConverterBase<Rank>
    {
        #region IValueConverter Members

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param><param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param><param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        protected override object Convert(Rank value, Type targetType, object parameter)
        {
            int cardValue = (int)value;
            // check the card is in the valid range
            if (cardValue > -1 && cardValue < 13)
            {
                string name = string.Empty;
                // recall that the cards are sorted by their game value.
                if (cardValue < 9 && cardValue > -1)
                    name = "" + (cardValue + 2);
                else
                {
                    switch (cardValue)
                    {
                        case 9: name = "J"; break;
                        case 10: name = "Q"; break;
                        case 11: name = "K"; break;
                        case 12: name = "A"; break;
                    }
                }

                return name;
            }

            return Binding.DoNothing;
        }

        /// <summary>Converts a value. </summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
		public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}