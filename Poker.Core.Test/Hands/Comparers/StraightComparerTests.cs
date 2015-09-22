using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Core.Deck;
using Poker.Core.Hands;
using Poker.Core.Hands.Comparers;

namespace Poker.Core.Test.Hands.Comparers
{
    /// <summary>
    /// Tests for the StraightComparer class.
    /// </summary>
    [TestClass]
    public class StraightComparerTests : ComparerTestBase<StraightComparer>
    {
        private StraightComparer _target;

        protected override StraightComparer Target
        {
            get { return _target ?? (_target = new StraightComparer()); }
            set { _target = value; }
        }

        protected override Core.Hands.Hands Type => Core.Hands.Hands.Straight;

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
        /// Test for the CompareHandHand when TypesEqual.
        /// </summary>
        [TestMethod]
        public void CompareHandHand_TypesEqual_HighCardEqual_ReturnsZero()
        {
            // ARRANGE
            var hand1 = Substitute.For<IHand>();
            hand1.Type.Returns(Type);
            hand1.HighCard.Returns(Rank.Ace);
            var hand2 = Substitute.For<IHand>();
            hand2.Type.Returns(Type);
            hand2.HighCard.Returns(Rank.Ace);

            // ACT
            var actual = Target.Compare(hand1, hand2);

            // ASSERT
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        /// Test for the Compare when IsStraight.
        /// </summary>
        [TestMethod]
        public void Compare_IsStraight_ReturnsStraightAndHighCard()
        {
            // ARRANGE
            var fakeHand = GetFakeHand(
                new Card(Rank.Queen, Suit.Spades),
                new Card(Rank.Jack, Suit.Spades),
                new Card(Rank.Ten, Suit.Hearts),
                new Card(Rank.Nine, Suit.Hearts),
                new Card(Rank.Eight, Suit.Hearts));

            // ACT
            var actual = Target.Compare(fakeHand);

            // ASSERT
            Assert.AreEqual(Type, actual.Item1);
            Assert.AreEqual(Rank.Queen, actual.Item2);
        }

        /// <summary>
        /// Test for the Compare when IsNotPair.
        /// </summary>
        [TestMethod]
        public void Compare_IsNotStraight_ReturnsSentinal()
        {
            // ARRANGE
            var fakeHand = GetFakeHand(
                new Card(Rank.Queen, Suit.Spades),
                new Card(Rank.Jack, Suit.Spades),
                new Card(Rank.Ten, Suit.Hearts),
                new Card(Rank.Nine, Suit.Hearts),
                new Card(Rank.Seven, Suit.Hearts));

            // ACT
            var actual = Target.Compare(fakeHand);

            // ASSERT
            Assert.AreEqual(0, (int)actual.Item1);
            Assert.AreEqual(0, (int)actual.Item2);
        }
    }
}