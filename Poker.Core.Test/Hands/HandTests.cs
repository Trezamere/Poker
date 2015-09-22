using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Core.Deck;
using Poker.Core.Hands;
using Poker.Core.Hands.Comparers;

namespace Poker.Core.Test.Hands
{
    /// <summary>
    /// Tests for the Hand class.
    /// </summary>
    [TestClass]
    public class HandTests
    {
        private Hand _target;

        protected Hand Target
        {
            get { return _target ?? (_target = new Hand(FakeCards, FakeCardComparer, FakeHandComparer)); }
            set { _target = value; }
        }

        public List<Card> FakeCards { get; set; }
        public IComparer<Card> FakeCardComparer { get; set; }
        public IHandComparer FakeHandComparer { get; set; }

        /// <summary>
        /// Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            FakeCards = new List<Card>
                {
                    new Card(Rank.Two, Suit.Hearts),
                    new Card(Rank.Nine, Suit.Diamonds),
                    new Card(Rank.Nine, Suit.Hearts),
                    new Card(Rank.Jack, Suit.Spades),
                    new Card(Rank.Ace, Suit.Clubs),
                };

            FakeCardComparer = Substitute.For<IComparer<Card>>();
            FakeHandComparer = Substitute.For<IHandComparer>();
            FakeHandComparer.Compare(Arg.Any<IHand>()).Returns(Tuple.Create(Core.Hands.Hands.OnePair, Rank.Nine));
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
        /// Test for the Constructor when SortsCards.
        /// </summary>
        [TestMethod]
        public void Constructor_SortsCards()
        {
            // ARRANGE

            // ACT
            var init = Target;

            // ASSERT
            FakeCardComparer.Received().Compare(Arg.Any<Card>(), Arg.Any<Card>());
        }

        /// <summary>
        /// Test for the Constructor when GetsBestHand.
        /// </summary>
        [TestMethod]
        public void Constructor_GetsBestHand()
        {
            // ARRANGE

            // ACT
            var init = Target;

            // ASSERT
            FakeHandComparer.Received().Compare(Target);
            Assert.AreEqual(Core.Hands.Hands.OnePair, Target.Type);
            Assert.AreEqual(Rank.Nine, Target.HighCard);
        }

        /// <summary>
        /// Test for the Add when ValidCard.
        /// </summary>
        [TestMethod]
        public void Add_ValidCard_AddsCard()
        {
            // ARRANGE
            var expected = FakeCards.Count;
            var fakeCard = new Card(Rank.Ace, Suit.Hearts);

            // ACT
            Target.Add(fakeCard);

            // ASSERT
            Assert.AreEqual(expected + 1, FakeCards.Count);
            Assert.AreEqual(fakeCard, FakeCards[expected]);
        }

        /// <summary>
        /// Test for the Add when ValidCard.
        /// </summary>
        [TestMethod]
        public void Add_ValidCard_SortsAfterAdd()
        {
            // ARRANGE
            var fakeCard = new Card(Rank.Ace, Suit.Hearts);

            // ACT
            Target.Add(fakeCard);

            // ASSERT
            FakeCardComparer.Received().Compare(Arg.Any<Card>(), Arg.Any<Card>());
        }

        /// <summary>
        /// Test for the Add when ValidCard.
        /// </summary>
        [TestMethod]
        public void Add_ValidCard_GetsBestHandAfterAdd()
        {
            // ARRANGE
            var fakeCard = new Card(Rank.Ace, Suit.Hearts);
            FakeHandComparer.Compare(Target).Returns(Tuple.Create(Core.Hands.Hands.TwoPair, Rank.Ace));

            // ACT
            Target.Add(fakeCard);

            // ASSERT
            FakeHandComparer.Received().Compare(Target);
            Assert.AreEqual(Core.Hands.Hands.TwoPair, Target.Type);
            Assert.AreEqual(Rank.Ace, Target.HighCard);
        }

        /// <summary>
        /// Test for the Discard when ValidCard.
        /// </summary>
        [TestMethod]
        public void Discard_ValidCard_RemovesCardAndReturnsTrue()
        {
            // ARRANGE
            var fakeCard = FakeCards[0];
            var expected = FakeCards.Count;

            // ACT
            var actual = Target.Discard(fakeCard);

            // ASSERT
            Assert.AreEqual(true, actual);
            Assert.AreEqual(expected-1, FakeCards.Count);
        }

        /// <summary>
        /// Test for the Discard when ValidCard.
        /// </summary>
        [TestMethod]
        public void Discard_ValidCard_SortsAfterRemoveAndReturnsTrue()
        {
            // ARRANGE
            var fakeCard = FakeCards[0];

            // ACT
            var actual = Target.Discard(fakeCard);

            // ASSERT
            Assert.AreEqual(true, actual);
            FakeCardComparer.Received().Compare(Arg.Any<Card>(), Arg.Any<Card>());
        }

        /// <summary>
        /// Test for the Discard when ValidCard.
        /// </summary>
        [TestMethod]
        public void Discard_ValidCard_GetsBestHandAfterRemoveAndReturnsTrue()
        {
            // ARRANGE
            var fakeCard = FakeCards[0];
            FakeHandComparer.Compare(Target).Returns(Tuple.Create(Core.Hands.Hands.HighCard, Rank.Ace));

            // ACT
            var actual = Target.Discard(fakeCard);

            // ASSERT
            Assert.AreEqual(true, actual);
            FakeHandComparer.Received().Compare(Target);
            Assert.AreEqual(Core.Hands.Hands.HighCard, Target.Type);
            Assert.AreEqual(Rank.Ace, Target.HighCard);
        }

        /// <summary>
        /// Test for the Discard when InvalidCard.
        /// </summary>
        [TestMethod]
        public void Discard_InvalidCard_ReturnsFalse()
        {
            // ARRANGE
            

            // ACT
            var actual = Target.Discard(Card.Blank);

            // ASSERT
            Assert.AreEqual(false, actual);
        }
    }
}