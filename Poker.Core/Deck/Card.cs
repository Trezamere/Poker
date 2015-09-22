using System;
using Catel.Data;

namespace Poker.Core.Deck
{
    /// <summary>
    /// A playing card consisting of a <see cref="Rank"/> and <see cref="Suit"/>.
    /// </summary>
    public class Card : ModelBase, IEquatable<Card>
    {
        /// <summary>
        /// Identifies a blank card.
        /// </summary>
        public static readonly Card Blank = new Card();

        /// <summary>
        /// Flag used in comparisons to determine equality with the blank card.
        /// </summary>
        private readonly bool _isBlank;

        /// <summary>
        /// Gets the <see cref="Rank"/> of this card.
        /// </summary>
        public Rank Rank { get; }

        /// <summary>
        /// Gets the <see cref="Suit"/> of this card.
        /// </summary>
        public Suit Suit { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        private Card()
        {
            _isBlank = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        public Card(Rank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
        }

        #region Equality members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Card other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _isBlank == other._isBlank && Rank == other.Rank && Suit == other.Suit;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="T:System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.
        ///             </exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Card) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ _isBlank.GetHashCode();
                hashCode = (hashCode*397) ^ (int) Rank;
                hashCode = (hashCode*397) ^ (int) Suit;
                return hashCode;
            }
        }

        public static bool operator ==(Card left, Card right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Card left, Card right)
        {
            return !Equals(left, right);
        }

        #endregion

        #region IsSelected property

        /// <summary>
        /// Identifies the <see cref="IsSelected"/> property.
        /// </summary>
        public static readonly PropertyData IsSelectedProperty = RegisterProperty("IsSelected", typeof(bool));

        /// <summary>
        /// Gets or sets a value indicating the <see cref="Card"/> is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return GetValue<bool>(IsSelectedProperty); }
            private set { SetValue(IsSelectedProperty, value); }
        }

        #endregion
    }
}
