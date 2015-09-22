using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Core.Deck;

namespace Poker.Core.Test.Deck
{
    /// <summary>
    /// Tests for the Card class.
    /// </summary>
    [TestClass]
    public class CardTests
    {
        private Card _target;
        protected Card Target
        {
            get { return _target ?? (_target = new Card(Rank.Ace, Suit.Clubs)); }
            set { _target = value; }
        }

        /// <summary>
        /// Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize() {}

        /// <summary>
        /// Use TestCleanup to run code after each test has run.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _target = null;
        }

        /// <summary>
        /// Test for the MethodUnderTest when Scenario.
        /// </summary>
        [TestMethod]
        public void Equals_NullReference_ReturnsFalse()
        {
            // ARRANGE
            
            // ACT
            var actual = Target.Equals(null);

            // ASSERT
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Test for the Equals when SameReference.
        /// </summary>
        [TestMethod]
        public void Equals_SameReference_ReturnsTrue()
        {
            // ARRANGE

            // ACT
            var actual = Target.Equals(Target);

            // ASSERT
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Test for the Equals when BlankNotEqualToTwoOfClubs.
        /// </summary>
        [TestMethod]
        public void Equals_BlankNotEqualToTwoOfClubs_ReturnsFalse()
        {
            // ARRANGE
            
            // ACT
            var actual = Card.Blank.Equals(new Card(0, 0));

            // ASSERT
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Test for the Equals when ValuesDifferent.
        /// </summary>
        [TestMethod]
        public void Equals_ValuesDifferent_ReturnsFalse()
        {
            // ARRANGE

            // ACT
            var actual = Target.Equals(new Card(Target.Rank, Target.Suit+1));

            // ASSERT
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Test for the Equals when ValuesSame.
        /// </summary>
        [TestMethod]
        public void Equals_ValuesSame_ReturnsTrue()
        {
            // ARRANGE

            // ACT
            var actual = Target.Equals(new Card(Target.Rank, Target.Suit));

            // ASSERT
            Assert.AreEqual(true, actual);
        }
    }
}