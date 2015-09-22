using System;
using System.Collections.Generic;
using System.Linq;
using Catel.IoC;

namespace Poker.Core.Deck
{
    /// <summary>
    /// Factory responsible for creating decks.
    /// </summary>
    [ServiceLocatorRegistration(typeof(IDeckFactory))]
    public class DeckFactory : IDeckFactory
    {
        /// <summary>
        /// Creates a standard 52-card deck.
        /// </summary>
        public IDeck CreateStandardDeck()
        {
            var cards = new List<Card>(52);
            cards.AddRange(from Suit suit in Enum.GetValues(typeof (Suit))
                           from Rank rank in Enum.GetValues(typeof (Rank))
                           select new Card(rank, suit));

            return new Deck(cards, new Random());
        }
    }
}