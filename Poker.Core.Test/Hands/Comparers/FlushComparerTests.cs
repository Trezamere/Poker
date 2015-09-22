using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Core.Deck;
using Poker.Core.Hands;
using Poker.Core.Hands.Comparers;

namespace Poker.Core.Test.Hands.Comparers
{
    /// <summary>
    /// Tests for the FlushComparer class.
    /// </summary>
    [TestClass]
    public class FlushComparerTests : ComparerTestBase<FlushComparer>
    {
        private FlushComparer _target;
        protected override FlushComparer Target
        {
            get { return _target ?? (_target = new FlushComparer(FakeHighCardComparer)); }
            set { _target = value; }
        }

        protected override Core.Hands.Hands Type => Core.Hands.Hands.Flush;

        public IHandComparer FakeHighCardComparer { get; set; }

        /// <summary>
        /// Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            FakeHighCardComparer = Substitute.For<IHandComparer>();
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
        public void CompareHandHand_TypesEqual_HighCardComparison()
        {
            // ARRANGE
            var hand1 = Substitute.For<IHand>();
            hand1.Type.Returns(Type);
            var hand2 = Substitute.For<IHand>();
            hand2.Type.Returns(Type);

            // ACT
            var actual = Target.Compare(hand1, hand2);

            // ASSERT
            FakeHighCardComparer.Received().Compare(hand1, hand2);
        }

        /// <summary>
        /// Test for the Compare when IsFlush.
        /// </summary>
        [TestMethod]
        public void Compare_IsFlush_ReturnsFlushAndHighCard()
        {
            // ARRANGE
            var fakeHand = GetFakeHand(
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Seven, Suit.Spades),
                new Card(Rank.Six, Suit.Spades),
                new Card(Rank.Five, Suit.Spades),
                new Card(Rank.Four, Suit.Spades));

            // ACT
            var actual = Target.Compare(fakeHand);

            // ASSERT
            Assert.AreEqual(Type, actual.Item1);
            Assert.AreEqual(Rank.Eight, actual.Item2);
        }

        /// <summary>
        /// Test for the Compare when IsNotFlush.
        /// </summary>
        [TestMethod]
        public void Compare_IsNotFlush_ReturnsSentinal()
        {
            // ARRANGE
            var fakeHand = GetFakeHand(
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Seven, Suit.Spades),
                new Card(Rank.Six, Suit.Spades),
                new Card(Rank.Five, Suit.Spades),
                new Card(Rank.Four, Suit.Hearts));

            // ACT
            var actual = Target.Compare(fakeHand);

            // ASSERT
            Assert.AreEqual(0, (int)actual.Item1);
            Assert.AreEqual(0, (int)actual.Item2);
        }
    }
}