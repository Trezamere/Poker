using System.Windows;
using System.Windows.Controls;
using Poker.Core.Deck;

namespace Poker.Application.Resources.Cards
{
    /// <summary>
    /// Selects the correct card art.
    /// </summary>
    public class CardArtTemplateSelector : DataTemplateSelector
    {
        private static readonly string[] ResourcesKeys =
            {
                "TwoCardTemplate",
                "ThreeCardTemplate",
                "FourCardTemplate",
                "FiveCardTemplate",
                "SixCardTemplate",
                "SevenCardTemplate",
                "EightCardTemplate",
                "NineCardTemplate",
                "TenCardTemplate",
                "JackCardTemplate",
                "QueenCardTemplate",
                "KingCardTemplate",
                "AceCardTemplate"
            };

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
                base.SelectTemplate(item, container);

            Card currentCard = (Card)item;
            FrameworkElement element = (FrameworkElement)container;

            string key = ResourcesKeys[(int) currentCard.Rank];
        
            return element.TryFindResource(key) as DataTemplate;

        }
    }
}
