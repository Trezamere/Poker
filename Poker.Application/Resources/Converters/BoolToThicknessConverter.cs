using System;
using System.Windows;
using Catel.MVVM.Converters;

namespace Poker.Application.Resources.Converters
{
    public class BoolToThicknessConverter : ValueConverterBase<bool, Thickness>
    {
        /// <summary>
        /// Gets or sets the Thickness value to set when the input value is true.
        /// </summary>
        public Thickness? TrueValue { get; set; }

        /// <summary>
        /// Gets or sets the Thickness value to set when the input value is false.
        /// </summary>
        public Thickness? FalseValue { get; set; }

        #region IValueConverter Members

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param><param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param><param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        protected override object Convert(bool value, Type targetType, object parameter)
        {
            if (value && TrueValue.HasValue)
            {
                return TrueValue;
            }

            if (!value && FalseValue.HasValue)
            {
                return FalseValue;
            }

            return DependencyProperty.UnsetValue;
        }

        #endregion 
    }
}