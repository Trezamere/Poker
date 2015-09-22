using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Core.Deck;
using Poker.Core.Hands;

namespace Poker.Core.Test
{
    /// <summary>
    /// Tests for the Game class.
    /// </summary>
    [TestClass]
    public class GameTests
    {
        private Game _target;
        protected Game Target
        {
            get { return _target ?? (_target = new Game(FakeDeck, FakePlayers, FakePlayerHandComparer, FakeHandFactory)); }
            set { _target = value; }
        }

        private IDeck FakeDeck { get; set; }
        private ICollection<IPlayer> FakePlayers { get; set; }
        private IComparer<IPlayer> FakePlayerHandComparer { get; set; }
        private Func<List<Card>, IHand> FakeHandFactory { get; set; }

        /// <summary>
        /// Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            FakeDeck = Substitute.For<IDeck>();
            FakePlayers = new List<IPlayer> {Substitute.For<IPlayer>(), Substitute.For<IPlayer>()};
            FakePlayerHandComparer = Substitute.For<IComparer<IPlayer>>();
            FakeHandFactory = Substitute.For<Func<List<Card>, IHand>>();
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
        /// Test for the Start when ShufflesDeck.
        /// </summary>
        [TestMethod]
        public void Start_ShufflesDeck()
        {
            // ARRANGE

            // ACT
            Target.Start();

            // ASSERT
            FakeDeck.Received().Shuffle();
        }

        /// <summary>
        /// Test for the Start when DealCardsToPlayers.
        /// </summary>
        [TestMethod]
        public void Start_DealCardsToPlayers_Deals5CardsToAllPlayers()
        {
            // ARRANGE
            var firstHand = Substitute.For<IHand>();
            var secondHand = Substitute.For<IHand>();
            FakeHandFactory.Invoke(Arg.Any<List<Card>>()).Returns(firstHand, secondHand);

            // ACT
            Target.Start();

            // ASSERT
            FakeDeck.Received(10).Draw();
            Assert.AreEqual(FakePlayers.ElementAt(0).Hand, firstHand);
            Assert.AreEqual(FakePlayers.ElementAt(1).Hand, secondHand);
        }

        /// <summary>
        /// Test for the End when SortsPlayers.
        /// </summary>
        [TestMethod]
        public void End_SortsPlayers()
        {
            // ARRANGE
            var player1 = FakePlayers.ElementAt(0);
            player1.Name = "Player 1";
            var player2 = FakePlayers.ElementAt(1);
            player2.Name = "Player 2";

            // Swap places when called.
            FakePlayerHandComparer.Compare(player1, player2).Returns(1);

            // ACT
            var actual = Target.End();

            // ASSERT
            FakePlayerHandComparer.Received().Compare(player1, player2);
            Assert.AreEqual(actual.ElementAt(0), player2);
        }

        /// <summary>
        /// Test for the End when NoTie.
        /// </summary>
        [TestMethod]
        public void End_NoTie_WinningPlayerIndex0()
        {
            // ARRANGE

            var player1 = FakePlayers.ElementAt(0);
            player1.Name = "Player 1";
            player1.Hand.Type.Returns(Core.Hands.Hands.TwoPair);
            var player2 = FakePlayers.ElementAt(1);
            player2.Name = "Player 2";
            player2.Hand.Type.Returns(Core.Hands.Hands.Straight);

            // Swap places when called.
            FakePlayerHandComparer.Compare(player1, player2).Returns(1);

            // ACT
            var actual = Target.End();

            // ASSERT
            FakePlayerHandComparer.Received().Compare(player1, player2);
            Assert.AreEqual(actual.Count, 1);
            Assert.AreEqual(actual.ElementAt(0), player2);
        }

        /// <summary>
        /// Test for the End when NoTie.
        /// </summary>
        [TestMethod]
        public void End_Tie_TwoWinners()
        {
            // ARRANGE

            var player1 = FakePlayers.ElementAt(0);
            player1.Name = "Player 1";
            player1.Hand.Type.Returns(Core.Hands.Hands.Straight);
            player1.Hand.HighCard.Returns(Rank.Four);
            var player2 = FakePlayers.ElementAt(1);
            player2.Name = "Player 2";
            player2.Hand.Type.Returns(Core.Hands.Hands.Straight);
            player2.Hand.HighCard.Returns(Rank.Four);

            // Swap places when called.
            FakePlayerHandComparer.Compare(player1, player2).Returns(1);

            // ACT
            var actual = Target.End();

            // ASSERT
            FakePlayerHandComparer.Received().Compare(player1, player2);
            Assert.AreEqual(actual.Count, 2);
            Assert.AreEqual(actual.ElementAt(0), player2);
            Assert.AreEqual(actual.ElementAt(1), player1);
        }
    }
}