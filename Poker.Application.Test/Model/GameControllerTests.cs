using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Application.Model;
using Poker.Core;
using Poker.Core.Deck;

namespace Poker.Application.Test.Model
{
    /// <summary>
    /// Tests for the GameController class.
    /// </summary>
    [TestClass]
    public class GameControllerTests
    {
        private GameController _target;
        protected GameController Target
        {
            get { return _target ?? (_target = new GameController(FakeRoundResultFactory)); }
            set { _target = value; }
        }

        private Func<IEnumerable<IPlayer>, IEnumerable<IPlayer>, IRoundResult> FakeRoundResultFactory { get; set; }

        private IGame FakeGame { get; set; }


        /// <summary>
        /// Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            FakeRoundResultFactory = Substitute.For<Func<IEnumerable<IPlayer>, IEnumerable<IPlayer>, IRoundResult>>();
            FakeGame = Substitute.For<IGame>();
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
        /// Test for the StartNewGame when Valid.
        /// </summary>
        [TestMethod]
        public void StartNewGame_Valid_StartsGame()
        {
            // ARRANGE
            Assert.AreEqual(false, Target.IsGameStarted);

            // ACT
            Target.StartNewGame(FakeGame);

            // ASSERT
            Assert.AreEqual(true, Target.IsGameStarted);
        }

        /// <summary>
        /// Test for the StartRound when CurrentGameIsNull.
        /// </summary>
        [TestMethod]
        public void StartRound_CurrentGameIsNull_DoesNothing()
        {
            // ARRANGE
            Assert.AreEqual(false, Target.IsPlaying);

            // ACT
            Target.StartRound();

            // ASSERT
            Assert.AreEqual(false, Target.IsPlaying);
        }

        /// <summary>
        /// Test for the StartRound when HasGame.
        /// </summary>
        [TestMethod]
        public void StartRound_HasGame_StartsGameAndIsPlaying()
        {
            // ARRANGE
            Target.StartNewGame(FakeGame);

            // ACT
            Target.StartRound();

            // ASSERT
            FakeGame.Received().Start();
            Assert.AreEqual(true, Target.IsPlaying);
        }

        /// <summary>
        /// Test for the EndRound when NotPlaying.
        /// </summary>
        [TestMethod]
        public void EndRound_NotPlaying_ReturnsEmptyResult()
        {
            // ARRANGE
            Assert.AreEqual(false, Target.IsPlaying);

            // ACT
            var actual = Target.EndRound();

            // ASSERT
            FakeRoundResultFactory.Received().Invoke(Arg.Is<IEnumerable<IPlayer>>(t => !t.Any()), Arg.Is<IEnumerable<IPlayer>>(t => !t.Any()));
        }

        /// <summary>
        /// Test for the EndRound when IsPlaying.
        /// </summary>
        [TestMethod]
        public void EndRound_IsPlaying_ReturnsResultWithWinnersAndLosers()
        {
            // ARRANGE
            var fakePlayers = new ObservableCollection<IPlayer>() {Substitute.For<IPlayer>(), Substitute.For<IPlayer>()};

            FakeGame.End().Returns(fakePlayers.Take(1).ToList());

            Target.Players = fakePlayers;
            Target.StartNewGame(FakeGame);
            Target.StartRound();

            var fakeResult = Substitute.For<IRoundResult>();
            FakeRoundResultFactory.Invoke(null, null).ReturnsForAnyArgs(fakeResult);

            // ACT
            var actual = Target.EndRound();

            // ASSERT
            Assert.AreEqual(fakeResult, actual);
            FakeRoundResultFactory.Received().Invoke(Arg.Is<IEnumerable<IPlayer>>(t => t.Contains(fakePlayers[0])), Arg.Is<IEnumerable<IPlayer>>(t => t.Contains(fakePlayers[1])));
        }
    }
}