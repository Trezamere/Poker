using System;
using System.Collections.Generic;

namespace Poker.Core.Deck
{
    /// <summary>
    /// A deck of cards.
    /// </summary>
    public class Deck : IDeck
    {
        /// <summary>
        /// All of the cards comprising this deck.
        /// </summary>
        private readonly IList<Card> _cards;

        /// <summary>
        /// The random number generator used to shuffle the deck.
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// The index for the top of the deck.
        /// </summary>
        private int _top;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class.
        /// </summary>
        public Deck(IList<Card> cards, Random random)
        {
            _cards = cards;
            _random = random;
        }

        /// <summary>
        /// Draws a card from the top of the deck.
        /// </summary>
        /// <returns>the <see cref="Card"/> from the top of the deck.</returns>
        /// <exception cref="InvalidOperationException">thrown if all of the cards in the deck were delt.</exception>
        public Card Draw()
        {
            if (_top >= _cards.Count)
                throw new InvalidOperationException("There are no more cards left in the deck.");

            return _cards[_top++];
        }

        /// <summary>
        /// Shuffles the entire deck.
        /// </summary>
        public void Shuffle()
        {
            int n = _cards.Count;
            while (n >= 1)
            {
                n--;
                int k = _random.Next(n + 1);
                var value = _cards[k];
                _cards[k] = _cards[n];
                _cards[n] = value;
            }

            _top = 0;
        }
    }
}