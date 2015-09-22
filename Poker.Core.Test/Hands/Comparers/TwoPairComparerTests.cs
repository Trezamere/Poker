using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Core.Deck;
using Poker.Core.Hands;
using Poker.Core.Hands.Comparers;

namespace Poker.Core.Test.Hands.Comparers
{
    /// <summary>
    /// Tests for the TwoPairComparer class.
    /// </summary>
    [TestClass]
    public class TwoPairComparerTests : ComparerTestBase<TwoPairComparer>
    {
        private TwoPairComparer _target;
        protected override TwoPairComparer Target
        {
            get { return _target ?? (_target = new TwoPairComparer()); }
            set { _target = value; }
        }

        protected override Core.Hands.Hands Type => Core.Hands.Hands.TwoPair;

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
        /// Test for the CompareHandHand when TypesEqual.
        /// </summary>
        [TestMethod]
        public void CompareHandHand_TypesEqual_ReturnsHighCard()
        {
            // ARRANGE
            var hand1 = Substitute.For<IHand>();
            hand1.Type.Returns(Type);
            hand1.HighCard.Returns(Rank.Ace);
            var hand2 = Substitute.For<IHand>();
            hand2.Type.Returns(Type);
            hand1.HighCard.Returns(Rank.Eight);

            // ACT
            var actual = Target.Compare(hand1, hand2);

            // ASSERT
            Assert.AreEqual(-1, actual);
        }

        /// <summary>
        /// Test for the CompareHandHand when HighCardsEqual.
        /// </summary>
        [TestMethod]
        public void CompareHandHand_HighCardsEqual_ReturnsSecondPairHighCard()
        {
            // ARRANGE
            var hand1 = GetFakeHand(
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Eight, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts));

            var hand2 = GetFakeHand(
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Eight, Suit.Hearts),
                new Card(Rank.Three, Suit.Hearts),
                new Card(Rank.Three, Suit.Hearts));

            // ACT
            var actual = Target.Compare(hand1, hand2);

            // ASSERT
            Assert.AreEqual(-1, actual);
        }

        /// <summary>
        /// Test for the CompareHandHand when SecondPairEqual.
        /// </summary>
        [TestMethod]
        public void CompareHandHand_SecondPairEqual_ReturnsHighCard_Hand2()
        {
            // ARRANGE
            var hand1 = GetFakeHand(
                new Card(Rank.Nine, Suit.Spades),
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Eight, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts));

            var hand2 = GetFakeHand(
                new Card(Rank.King, Suit.Spades),
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Eight, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts));

            // ACT
            var actual = Target.Compare(hand1, hand2);

            // ASSERT
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Test for the CompareHandHand when HandsEqual.
        /// </summary>
        [TestMethod]
        public void CompareHandHand_HandsEqual_ReturnsZero()
        {
            // ARRANGE
            var hand1 = GetFakeHand(
                new Card(Rank.Nine, Suit.Spades),
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Eight, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts));

            var hand2 = GetFakeHand(
                new Card(Rank.Nine, Suit.Spades),
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Eight, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts));

            // ACT
            var actual = Target.Compare(hand1, hand2);

            // ASSERT
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        /// Test for the Compare when IsTwoPair.
        /// </summary>
        [TestMethod]
        public void Compare_IsTwoPair_ReturnsTwoPairAndHighPair()
        {
            // ARRANGE
            var fakeHand = GetFakeHand(
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Eight, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts));

            // ACT
            var actual = Target.Compare(fakeHand);

            // ASSERT
            Assert.AreEqual(Type, actual.Item1);
            Assert.AreEqual(Rank.Eight, actual.Item2);
        }

        /// <summary>
        /// Test for the Compare when IsNotTwoPair.
        /// </summary>
        [TestMethod]
        public void Compare_IsNotTwoPair_ReturnsSentinal()
        {
            // ARRANGE
            var fakeHand = GetFakeHand(
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Three, Suit.Spades),
                new Card(Rank.Two, Suit.Spades));

            // ACT
            var actual = Target.Compare(fakeHand);

            // ASSERT
            Assert.AreEqual(0, (int)actual.Item1);
            Assert.AreEqual(0, (int)actual.Item2);
        }
    }
}