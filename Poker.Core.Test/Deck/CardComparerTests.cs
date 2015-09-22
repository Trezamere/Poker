using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Core.Deck;

namespace Poker.Core.Test.Deck
{
    /// <summary>
    /// Tests for the CardComparer class.
    /// </summary>
    [TestClass]
    public class CardComparerTests
    {
        private CardComparer _target;

        protected CardComparer Target
        {
            get { return _target ?? (_target = new CardComparer()); }
            set { _target = value; }
        }

        /// <summary>
        /// Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
        }

        /// <summary>
        /// Use TestCleanup to run code after each test has run.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _target = null;
        }

        /// <summary>
        /// Test for the Compare when CardXIsBetter.
        /// </summary>
        [TestMethod]
        public void Compare_CardXIsBetter_ReturnsNegativeOne()
        {
            // ARRANGE
            var x = new Card(Rank.Ace, Suit.Hearts);
            var y = new Card(Rank.King, Suit.Hearts);

            // ACT
            var actual = Target.Compare(x, y);

            // ASSERT
            Assert.AreEqual(-1, actual);
        }

        /// <summary>
        /// Test for the Compare when CardYIsBetter.
        /// </summary>
        [TestMethod]
        public void Compare_CardYIsBetter_ReturnsOne()
        {
            // ARRANGE
            var x = new Card(Rank.Queen, Suit.Hearts);
            var y = new Card(Rank.King, Suit.Hearts);

            // ACT
            var actual = Target.Compare(x, y);

            // ASSERT
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Test for the Compare when CardsAreEqual.
        /// </summary>
        [TestMethod]
        public void Compare_CardsAreEqual_ReturnsZero()
        {
            // ARRANGE
            var x = new Card(Rank.Queen, Suit.Hearts);
            var y = new Card(Rank.Queen, Suit.Hearts);

            // ACT
            var actual = Target.Compare(x, y);

            // ASSERT
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        /// Test for the Compare when CardsEqualAndSuitDoesNotMatter.
        /// </summary>
        [TestMethod]
        public void Compare_CardsEqualAndSuitDoesNotMatter_ReturnsZero()
        {
            // ARRANGE
            var x = new Card(Rank.Queen, Suit.Hearts);
            var y = new Card(Rank.Queen, Suit.Clubs);

            // ACT
            var actual = Target.Compare(x, y);

            // ASSERT
            Assert.AreEqual(0, actual);
        }
    }
}