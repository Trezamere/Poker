using System;
using System.Windows.Controls;
using System.Windows.Data;
using Poker.Core.Deck;

namespace Poker.Application.Resources.Cards.Controls
{
    /// <summary>
    /// Draws the suit of the card according to the content, which is assumed to be a <see cref="Card"/>.
    /// </summary>
    public class SuiteControl : ContentControl
    {
        /// <summary>Raises the <see cref="E:System.Windows.FrameworkElement.Initialized"/> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized"/> is set to true internally. </summary>
        /// <returns/>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs"/> that contains the event data.</param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Inherit the content from containing control
            BindingOperations.SetBinding(this, ContentControl.ContentProperty, new Binding());
        }

        /// <summary>
        /// Called when the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property changes. 
        /// </summary>
        /// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param><param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            Card currentCard;
            if (newContent is Card)
                currentCard = (Card) newContent;
            else
                return;

            if (currentCard.Suit == Suit.Clubs)
                Template = TryFindResource("Clubs") as ControlTemplate;
            else if (currentCard.Suit == Suit.Diamonds)
                Template = TryFindResource("Diamonds") as ControlTemplate;
            else if (currentCard.Suit == Suit.Hearts)
                Template = TryFindResource("Hearts") as ControlTemplate;
            else if (currentCard.Suit == Suit.Spades)
                Template = TryFindResource("Spades") as ControlTemplate;
        }
    }
}