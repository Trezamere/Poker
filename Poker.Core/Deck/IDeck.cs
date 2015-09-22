namespace Poker.Core.Deck
{
    /// <summary>
    /// Defines a deck of cards.
    /// </summary>
    public interface IDeck
    {
        /// <summary>
        /// Draws a card from the top of the deck.
        /// </summary>
        /// <returns>the <see cref="Card"/> from the top of the deck.</returns>
        Card Draw();

        /// <summary>
        /// Shuffles the entire deck.
        /// </summary>
        void Shuffle();
    }
}