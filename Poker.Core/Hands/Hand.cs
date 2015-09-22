using System;
using System.Collections;
using System.Collections.Generic;
using Catel;
using Poker.Core.Deck;
using Poker.Core.Hands.Comparers;

namespace Poker.Core.Hands
{
    /// <summary>
    /// A collection of cards which are compared to other hands using a standard ranking system to determine the winner of a particular deal.
    /// <para>The cards are kept in order from Ace (high) to Two (low).</para>
    /// </summary>
    public class Hand : IHand
    {
        /// <summary>
        /// The collection of cards comprising this hand.
        /// </summary>
        private readonly List<Card> _cards;

        /// <summary>
        /// The comparer which handles sorting the hand.
        /// </summary>
        private readonly IComparer<Card> _cardComparer;

        /// <summary>
        /// The comparer which handles choosing the best possible hand.
        /// </summary>
        private readonly IHandComparer _handComparer;

        /// <summary>
        /// Gets the number of cards contained in the hand.
        /// </summary>
        public int Count => _cards.Count;

        /// <summary>
        /// Gets the card at the specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">index is less than 0.-or-index is equal to or greater than <see cref="Count"/>.</exception>
        public Card this[int i] => _cards[i];

        /// <summary>
        /// Gets the type of hand. Two Pair, Flush etc.
        /// </summary>
        public Hands Type { get; private set; }

        /// <summary>
        /// Gets the high card for this hand.
        /// </summary>
        public Rank HighCard { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hand"/> class.
        /// </summary>
        public Hand(List<Card> cards, IComparer<Card> cardComparer, IHandComparer handComparer)
        {
            Argument.IsNotNull(() => cards);
            Argument.IsNotNull(() => cardComparer);
            Argument.IsNotNull(() => handComparer);

            _cards = cards;
            _cardComparer = cardComparer;
            _handComparer = handComparer;

            // Perform the initial sort.
            _cards.Sort(_cardComparer);

            // Perform the intial comparison
            var comparison = _handComparer.Compare(this);
            Type = comparison.Item1;
            HighCard = comparison.Item2;
        }

        /// <summary>
        /// Adds the specified card to the hand.
        /// </summary>
        /// <param name="card">The card to add to the hand.</param>
        /// <returns>true if card is added to the hand; otherwise, false.</returns>
        public void Add(Card card)
        {
            Argument.IsNotNull(() => card);

            _cards.Add(card);

            // Need to re-sort and get the best hand after every addition.
            _cards.Sort(_cardComparer);
            var comparison = _handComparer.Compare(this);
            Type = comparison.Item1;
            HighCard = comparison.Item2;
        }

        /// <summary>
        /// Removes a specified card from the hand.
        /// </summary>
        /// <param name="card">The card to remove.</param>
        /// <returns>true if the card is found and successfully removed; otherwise, false.</returns>
        public bool Discard(Card card)
        {
            Argument.IsNotNull(() => card);

            var removed = _cards.Remove(card);
            if (removed)
            {
                // Need to re-sort and get the best hand after every deletion.
                _cards.Sort(_cardComparer);
                var comparison = _handComparer.Compare(this);
                Type = comparison.Item1;
                HighCard = comparison.Item2;
            }

            return removed;
        }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        #endregion

        #region Implementation of IComparable<in Hand>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(IHand other)
        {
            return _handComparer.Compare(this, other);
        }

        #endregion

        #region Implementation of IEnumerable<out Card>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<Card> IEnumerable<Card>.GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<Card>

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        void ICollection<Card>.Clear()
        {
            _cards.Clear();

            Type = 0;
            HighCard = 0;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        bool ICollection<Card>.Contains(Card item)
        {
            return _cards.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        void ICollection<Card>.CopyTo(Card[] array, int arrayIndex)
        {
            _cards.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        bool ICollection<Card>.Remove(Card item)
        {
            return Discard(item);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly => false;

        #endregion
    }
}