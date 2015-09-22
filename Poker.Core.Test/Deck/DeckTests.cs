using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Core.Deck;

namespace Poker.Core.Test.Deck
{
    /// <summary>
    /// Tests for the Deck class.
    /// </summary>
    [TestClass]
    public class DeckTests
    {
        private Core.Deck.Deck _target;
        protected Core.Deck.Deck Target
        {
            get { return _target ?? (_target = new Core.Deck.Deck(FakeCards, FakeRandom)); }
            set { _target = value; }
        }

        public IList<Card> FakeCards { get; set; }
        public Random FakeRandom { get; set; }

        /// <summary>
        /// Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            FakeCards = new List<Card>
                {
                    new Card(Rank.Four, Suit.Diamonds),
                    new Card(Rank.Five, Suit.Diamonds),
                    new Card(Rank.Four, Suit.Clubs)
                };
            FakeRandom = Substitute.For<Random>();
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
        /// Test for the Shuffle when SwapsCardsCountTimes.
        /// </summary>
        [TestMethod]
        public void Shuffle_SwapsCardsCountTimes_CallNext3Times()
        {
            // ARRANGE

            // ACT
            Target.Shuffle();

            // ASSERT
            FakeRandom.Received(3).Next(Arg.Any<int>());
        }

        /// <summary>
        /// Test for the Draw when DrawsNextCard.
        /// </summary>
        [TestMethod]
        public void Draw_DrawsNextCard_IteratesCorrectCards()
        {
            // ARRANGE

            // ACT

            // ASSERT
            foreach (Card expected in FakeCards) {
                Assert.AreEqual(Target.Draw(), expected);
            }
        }

        /// <summary>
        /// Test for the Draw when DrawsAllCards.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Draw_DrawsAllCards_ThrowsException()
        {
            // ARRANGE
            foreach (Card expected in FakeCards)
            {
                Target.Draw();
            }

            // ACT
            Target.Draw();

            // ASSERT
            Assert.Fail("Did not throw.");
        }
    }
}