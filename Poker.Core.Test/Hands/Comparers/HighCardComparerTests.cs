using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Core.Deck;
using Poker.Core.Hands.Comparers;

namespace Poker.Core.Test.Hands.Comparers
{
    /// <summary>
    /// Tests for the HighCardComparer class.
    /// </summary>
    [TestClass]
    public class HighCardComparerTests : ComparerTestBase<HighCardComparer>
    {
        private HighCardComparer _target;
        protected override HighCardComparer Target
        {
            get { return _target ?? (_target = new HighCardComparer(FakeCardComparer)); }
            set { _target = value; }
        }

        public IComparer<Card> FakeCardComparer { get; set; }

        protected override Core.Hands.Hands Type => Core.Hands.Hands.HighCard;
        protected override int MinCount => 0;

        /// <summary>
        /// Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            FakeCardComparer = Substitute.For<IComparer<Card>>();
        }

        /// <summary>
        /// Use TestCleanup to run code after each test has run.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _target = null;
        }

        #region Overrides of ComparerTestBase<HighCardComparer>

        /// <summary>
        /// Test for the CompareHandHand when TypesNotEqual.
        /// </summary>
        public override void CompareHandHand_TypesNotEqual_Throws()
        {
            // Do nothing, test not applicable
        }

        /// <summary>
        /// Test for the CompareHandHand when Scenario.
        /// </summary>
        public override void CompareHandHand_XHandNotEqualToType_YHandIsEqualType_Returns1()
        {
            // Do nothing, test not applicable
        }

        /// <summary>
        /// Test for the CompareHandHand when XHandIsEqualType.
        /// </summary>
        public override void CompareHandHand_XHandIsEqualType_YHandNotEqualToType_ReturnsNegativeOne()
        {
            // Do nothing, test not applicable
        }

        #endregion

        /// <summary>
        /// Test for the Compare when HasCards.
        /// </summary>
        [TestMethod]
        public void Compare_HasCards_ReturnsFirstCard()
        {
            // ARRANGE
            var fakeHand = GetFakeHand(
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Seven, Suit.Spades),
                new Card(Rank.Five, Suit.Spades),
                new Card(Rank.Three, Suit.Spades),
                new Card(Rank.Two, Suit.Spades));

            // ACT
            var actual = Target.Compare(fakeHand);

            // ASSERT
            Assert.AreEqual(Core.Hands.Hands.HighCard, actual.Item1);
            Assert.AreEqual(Rank.Eight, actual.Item2);
        }

        /// <summary>
        /// Test for the CompareHandHand when AllEqual.
        /// </summary>
        [TestMethod]
        public void CompareHandHand_AllEqual_ReturnsZero()
        {
            // ARRANGE
            var fakeHand1 = GetFakeHand(
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Seven, Suit.Spades),
                new Card(Rank.Five, Suit.Spades),
                new Card(Rank.Three, Suit.Spades),
                new Card(Rank.Two, Suit.Spades));

            var fakeHand2 = GetFakeHand(
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Seven, Suit.Spades),
                new Card(Rank.Five, Suit.Spades),
                new Card(Rank.Three, Suit.Spades),
                new Card(Rank.Two, Suit.Spades));

            // ACT
            var actual = Target.Compare(fakeHand1, fakeHand2);

            // ASSERT
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        /// Test for the CompareHandHand when NotEqual.
        /// </summary>
        [TestMethod]
        public void CompareHandHand_NotEqual_ReturnsCardComparisonValue()
        {
            // ARRANGE
            var fakeHand1 = GetFakeHand(
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Seven, Suit.Spades),
                new Card(Rank.Five, Suit.Spades),
                new Card(Rank.Three, Suit.Spades),
                new Card(Rank.Three, Suit.Spades));

            var fakeHand2 = GetFakeHand(
                new Card(Rank.Eight, Suit.Spades),
                new Card(Rank.Seven, Suit.Spades),
                new Card(Rank.Five, Suit.Spades),
                new Card(Rank.Three, Suit.Spades),
                new Card(Rank.Two, Suit.Spades));

            FakeCardComparer.Compare(Arg.Any<Card>(), Arg.Any<Card>()).Returns(0, 0, 0, 0, -1);

            // ACT
            var actual = Target.Compare(fakeHand1, fakeHand2);

            // ASSERT
            Assert.AreEqual(-1, actual);
        }
    }
}