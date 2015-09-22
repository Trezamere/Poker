using System;
using System.Collections.Generic;
using Poker.Core.Deck;

namespace Poker.Core.Hands
{
    /// <summary>
    /// Defines properties and methods to access a hand of cards.
    /// <para>Hands are compared to other hands using a standard ranking system to determine the winner of a particular deal.</para>
    /// <para>The cards are kept in order from Ace (high) to Two (low).</para>
    /// </summary>
    public interface IHand : ICollection<Card>, IComparable<IHand>
    {
        /// <summary>
        /// Gets the number of cards contained in the hand.
        /// </summary>
        new int Count { get; }

        /// <summary>
        /// Gets the type of hand. Two Pair, Flush etc.
        /// </summary>
        Hands Type { get; }

        /// <summary>
        /// Gets the high card for this hand.
        /// </summary>
        Rank HighCard { get; }

        /// <summary>
        /// Gets the card at the specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">index is less than 0.-or-index is equal to or greater than <see cref="Count"/>.</exception>
        Card this[int i] { get; }

        /// <summary>
        /// Adds the specified card to the hand.
        /// </summary>
        /// <param name="card">The card to add to the hand.</param>
        /// <returns>true if card is added to the hand; otherwise, false.</returns>
        new void Add(Card card);

        /// <summary>
        /// Removes a specified card from the hand.
        /// </summary>
        /// <param name="card">The card to remove.</param>
        /// <returns>true if the card is found and successfully removed; otherwise, false.</returns>
        bool Discard(Card card);
    }
}