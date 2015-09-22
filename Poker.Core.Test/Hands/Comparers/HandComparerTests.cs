using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Core.Deck;
using Poker.Core.Hands;
using Poker.Core.Hands.Comparers;

namespace Poker.Core.Test.Hands.Comparers
{
    /// <summary>
    /// Tests for the HandComparer class.
    /// </summary>
    [TestClass]
    public class HandComparerTests
    {
        private HandComparer _target;
        protected HandComparer Target
        {
            get { return _target ?? (_target = new HandComparer(
                FakeHighCardComparer,
                FakePairComparer,
                FakeTwoPairComparer,
                FakeThreeOfAKindComparer,
                FakeStraightComparer,
                FakeFlushComparer,
                FakeFullHouseComparer,
                FakeFourOfAKindComparer,
                FakeStraightFlushComparer)); }
            set { _target = value; }
        }

        public IHandComparer FakeHighCardComparer { get; set; }
        public IHandComparer FakePairComparer { get; set; }
        public IHandComparer FakeTwoPairComparer { get; set; }
        public IHandComparer FakeThreeOfAKindComparer { get; set; }
        public IHandComparer FakeStraightComparer { get; set; }
        public IHandComparer FakeFlushComparer { get; set; }
        public IHandComparer FakeFullHouseComparer { get; set; }
        public IHandComparer FakeFourOfAKindComparer { get; set; }
        public IHandComparer FakeStraightFlushComparer { get; set; }

        /// <summary>
        /// Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            FakeHighCardComparer = Substitute.For<IHandComparer>();
            FakePairComparer = Substitute.For<IHandComparer>();
            FakeTwoPairComparer = Substitute.For<IHandComparer>();
            FakeThreeOfAKindComparer = Substitute.For<IHandComparer>();
            FakeStraightComparer = Substitute.For<IHandComparer>();
            FakeFlushComparer = Substitute.For<IHandComparer>();
            FakeFullHouseComparer = Substitute.For<IHandComparer>();
            FakeFourOfAKindComparer = Substitute.For<IHandComparer>();
            FakeStraightFlushComparer = Substitute.For<IHandComparer>();

            var sentinal = Tuple.Create<Core.Hands.Hands, Rank>(0, 0);
            FakeHighCardComparer.Compare(Arg.Any<IHand>()).Returns(sentinal);
            FakePairComparer.Compare(Arg.Any<IHand>()).Returns(sentinal);
            FakeTwoPairComparer.Compare(Arg.Any<IHand>()).Returns(sentinal);
            FakeThreeOfAKindComparer.Compare(Arg.Any<IHand>()).Returns(sentinal);
            FakeStraightComparer.Compare(Arg.Any<IHand>()).Returns(sentinal);
            FakeFlushComparer.Compare(Arg.Any<IHand>()).Returns(sentinal);
            FakeFullHouseComparer.Compare(Arg.Any<IHand>()).Returns(sentinal);
            FakeFourOfAKindComparer.Compare(Arg.Any<IHand>()).Returns(sentinal);
            FakeStraightFlushComparer.Compare(Arg.Any<IHand>()).Returns(sentinal);

            FakeHighCardComparer.WhenForAnyArgs(t => t.Compare(null, null)).Do((t) => { throw new InvalidOperationException(); });
            FakePairComparer.WhenForAnyArgs(t => t.Compare(null, null)).Do((t) => { throw new InvalidOperationException(); });
            FakeTwoPairComparer.WhenForAnyArgs(t => t.Compare(null, null)).Do((t) => { throw new InvalidOperationException(); });
            FakeThreeOfAKindComparer.WhenForAnyArgs(t => t.Compare(null, null)).Do((t) => { throw new InvalidOperationException(); });
            FakeStraightComparer.WhenForAnyArgs(t => t.Compare(null, null)).Do((t) => { throw new InvalidOperationException(); });
            FakeFlushComparer.WhenForAnyArgs(t => t.Compare(null, null)).Do((t) => { throw new InvalidOperationException(); });
            FakeFullHouseComparer.WhenForAnyArgs(t => t.Compare(null, null)).Do((t) => { throw new InvalidOperationException(); });
            FakeFourOfAKindComparer.WhenForAnyArgs(t => t.Compare(null, null)).Do((t) => { throw new InvalidOperationException(); });
            FakeStraightFlushComparer.WhenForAnyArgs(t => t.Compare(null, null)).Do((t) => { throw new InvalidOperationException(); });
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
        /// Helper method to build a fake hand.
        /// </summary>
        protected IHand GetFakeHand(Core.Hands.Hands type, params Card[] cards)
        {
            var fakeCards = new List<Card>(cards);

            var fakeHand = Substitute.For<IHand>();
            fakeHand.Count.Returns(fakeCards.Count);
            fakeHand.Type.Returns(type);
            fakeHand.HighCard.Returns(cards.Length == 0 ? Rank.Two : cards[0].Rank);
            fakeHand.GetEnumerator().Returns(fakeCards.GetEnumerator());
            fakeHand[Arg.Any<int>()].Returns(param => fakeCards[param.Arg<int>()]);
            return fakeHand;
        }

        /// <summary>
        /// Test for the Compare when ReturnsFirstMatchingComparerResult.
        /// </summary>
        [TestMethod]
        public void Compare_ReturnsFirstMatchingComparerResult_ReturnsStraight()
        {
            // ARRANGE
            var hand = GetFakeHand(Core.Hands.Hands.Straight);

            FakeStraightComparer.Compare(hand).Returns(Tuple.Create(Core.Hands.Hands.Straight, Rank.Eight));

            // ACT
            var actual = Target.Compare(hand);

            // ASSERT
            Assert.AreEqual(Core.Hands.Hands.Straight, actual.Item1);
            Assert.AreEqual(Rank.Eight, actual.Item2);
        }

        /// <summary>
        /// Test for the Compare when ReturnsFirstMatchingComparerResult.
        /// </summary>
        [TestMethod]
        public void Compare_AllEqual_ReturnsHighCardRank()
        {
            // ARRANGE
            var hand = GetFakeHand(Core.Hands.Hands.HighCard, new Card(Rank.King, Suit.Spades));

            FakeHighCardComparer.Compare(hand).Returns(Tuple.Create(Core.Hands.Hands.HighCard, Rank.King));

            // ACT
            var actual = Target.Compare(hand);

            // ASSERT
            Assert.AreEqual(Core.Hands.Hands.HighCard, actual.Item1);
            Assert.AreEqual(Rank.King, actual.Item2);
        }

        /// <summary>
        /// Test for the Compare when CountsMismatch.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CompareHandHand_CountsMismatch_Throws()
        {
            // ARRANGE
            var hand1 = Substitute.For<IHand>();
            hand1.Type.Returns(Core.Hands.Hands.Straight);
            hand1.HighCard.Returns(Rank.Jack);
            hand1.Count.Returns(5);
            var hand2 = Substitute.For<IHand>();
            hand2.Type.Returns(Core.Hands.Hands.Straight);
            hand2.HighCard.Returns(Rank.Jack);
            hand2.Count.Returns(6);

            // ACT
            var actual = Target.Compare(hand1, hand2);

            // ASSERT
            Assert.Fail("Did not throw.");
        }

        /// <summary>
        /// Test for the CompareHandHand when HandOneHasBetterHand.
        /// </summary>
        [TestMethod]
        public void CompareHandHand_HandOneHasBetterHand_ReturnsNegativeOne()
        {
            // ARRANGE
            var hand1 = Substitute.For<IHand>();
            hand1.Type.Returns(Core.Hands.Hands.Straight);
            hand1.HighCard.Returns(Rank.Jack);
            hand1.Count.Returns(5);
            var hand2 = Substitute.For<IHand>();
            hand2.Type.Returns(Core.Hands.Hands.TwoPair);
            hand2.HighCard.Returns(Rank.Jack);
            hand2.Count.Returns(5);

            // ACT
            var actual = Target.Compare(hand1, hand2);

            // ASSERT
            Assert.AreEqual(-1, actual);
        }

        /// <summary>
        /// Test for the CompareHandHand when HandOneHasBetterHand.
        /// </summary>
        [TestMethod]
        public void CompareHandHand_HandTwoHasBetterHand_ReturnsOne()
        {
            // ARRANGE
            var hand1 = Substitute.For<IHand>();
            hand1.Type.Returns(Core.Hands.Hands.HighCard);
            hand1.HighCard.Returns(Rank.Jack);
            hand1.Count.Returns(5);
            var hand2 = Substitute.For<IHand>();
            hand2.Type.Returns(Core.Hands.Hands.TwoPair);
            hand2.HighCard.Returns(Rank.Jack);
            hand2.Count.Returns(5);

            // ACT
            var actual = Target.Compare(hand1, hand2);

            // ASSERT
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Test for the CompareHandHand when HandsEqual.
        /// </summary>
        [TestMethod]
        public void CompareHandHand_HandsEqual_UsesComparerToDetermineBestHand()
        {
            // ARRANGE
            var hand1 = Substitute.For<IHand>();
            hand1.Type.Returns(Core.Hands.Hands.TwoPair);
            hand1.HighCard.Returns(Rank.Jack);
            hand1.Count.Returns(5);
            var hand2 = Substitute.For<IHand>();
            hand2.Type.Returns(Core.Hands.Hands.TwoPair);
            hand2.HighCard.Returns(Rank.Jack);
            hand2.Count.Returns(5);

            FakeTwoPairComparer = Substitute.For<IHandComparer>();
            FakeTwoPairComparer.Compare(hand1, hand2).Returns(1);

            // ACT
            var actual = Target.Compare(hand1, hand2);

            // ASSERT
            Assert.AreEqual(1, actual);
            FakePairComparer.DidNotReceiveWithAnyArgs().Compare(null, null);
        }
    }
}