using Catel.Data;
using Poker.Core.Deck;

namespace Poker.Application.Resources.Cards
{
    /// <summary>
    /// Represents a card played in a game.
    /// </summary>
    public class PlayingCard : ModelBase
    {
        /// <summary>
        /// Gets the underlying <see cref="Card"/> object for this playing card.
        /// </summary>
        public Card Card { get; }

        #region IsSelected property

        /// <summary>
        /// Identifies the <see cref="IsSelected"/> property.
        /// </summary>
        public static readonly PropertyData IsSelectedProperty = RegisterProperty("IsSelected", typeof(bool));

        /// <summary>
        /// Gets or sets a value indicating the <see cref="PlayingCard"/> is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return GetValue<bool>(IsSelectedProperty); }
            private set { SetValue(IsSelectedProperty, value); }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayingCard"/> class.
        /// </summary>
        /// <param name="card">The <see cref="Card"/> object this playing card will be wrapping.</param>
        public PlayingCard(Card card)
        {
            Card = card;   
        }
    }
}