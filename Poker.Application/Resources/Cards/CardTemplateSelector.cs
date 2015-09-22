using System.Windows;
using System.Windows.Controls;
using Poker.Core.Deck;

namespace Poker.Application.Resources.Cards
{
	public class CardTemplateSelector : DataTemplateSelector
	{
	    /// <summary>
	    /// When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate"/> based on custom logic.
	    /// </summary>
	    /// <returns>
	    /// Returns a <see cref="T:System.Windows.DataTemplate"/> or null. The default value is null.
	    /// </returns>
	    /// <param name="item">The data object for which to select the template.</param><param name="container">The data-bound object.</param>
	    public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item == null)
				return null;

			var currentCard = (Card)item;
            FrameworkElement element = (FrameworkElement)container;

			//if (currentCard == Card.Blank)
            //      return element.TryFindResource("CardBackTemplate") as DataTemplate;

            return element.TryFindResource("CardFrontTemplate") as DataTemplate;
		}
	}
}
