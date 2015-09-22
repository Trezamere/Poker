using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Core.Deck;
using Poker.Core.Hands;
using Poker.Core.Hands.Comparers;

namespace Poker.Core.Test.Hands.Comparers
{
    /// <summary>
    /// Tests for the PairComparer class.
    /// </summary>
    [TestClass]
    public class PairComparerTests : ComparerTestBase<PairComparer>
    {
        private PairComparer _target;
        protected override PairComparer Target
        {
            get { return _target ?? (_target = new PairComparer(FakeComparer)); }
            set { _target = value; }
        }

        protected override Core.Hands.Hands Type => Core.Hands.Hands.OnePair;

        public IHandComparer FakeComparer { get; set; }

        /// <summary>
        /// Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            FakeComparer = Substitute.For<IHandComparer>();
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
        /// Test for the CompareHandHand when TypesEqual.
        /// </summary>
        [TestMethod]
        public void CompareHandHand_TypesEqual_ReturnsComparisonValue()
        {
            // ARRANGE
            var hand1 = Substitute.For<IHand>();
            hand1.Type.Returns(Type);
            var hand2 = Substitute.For<IHand>();
            hand2.Type.Returns(Type);

            FakeComparer.Compare(hand1, hand2).Returns(1);

            // ACT
            var actual = Target.Compare(hand1, hand2);

            // ASSERT
            Assert.AreEqual(1, actual);
            FakeComparer.Received().Compare(hand1, hand2);
        }

        /// <summary>
        /// Test for the Compare when IsPair.
        /// </summary>
        [TestMethod]
        public void Compare_IsPair_ReturnsPairAndHighPair()
        {
            // ARRANGE
            var fakeHand = GetFakeHand(
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Four, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts),
                new Card(Rank.Four, Suit.Hearts));

            // ACT
            var actual = Target.Compare(fakeHand);

            // ASSERT
            Assert.AreEqual(Type, actual.Item1);
            Assert.AreEqual(Rank.Eight, actual.Item2);
        }

        /// <summary>
        /// Test for the Compare when IsNotPair.
        /// </summary>
        [TestMethod]
        public void Compare_IsNotPair_ReturnsSentinal()
        {
            // ARRANGE
            var fakeHand = GetFakeHand(
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.Nine, Suit.Spades),
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