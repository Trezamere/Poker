namespace Poker.Core.Deck
{
    /// <summary>
    /// Defines functions for creating decks.
    /// </summary>
    public interface IDeckFactory {
        /// <summary>
        /// Creates a standard 52-card deck.
        /// </summary>
        IDeck CreateStandardDeck();
    }
}