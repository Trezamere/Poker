using System;
using System.Collections.Generic;
using Catel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Application.Model;
using Poker.Application.ViewModel;
using Poker.Core;

namespace Poker.Application.Test.ViewModel
{
    /// <summary>
    /// Tests for the MainWindowViewModel class.
    /// </summary>
    [TestClass]
    public class MainWindowViewModelTests
    {
        private MainWindowViewModel _target;
        protected MainWindowViewModel Target
        {
            get { return _target ?? (_target = new MainWindowViewModel(
                FakeGameController,
                FakeUiVisualizerService,
                FakePlayerConfigurationFactory,
                FakeGameFactory,
                FakeRoundResultFactory)); }
            set { _target = value; }
        }

        private IGameController FakeGameController { get; set; }
        private IUIVisualizerService FakeUiVisualizerService { get; set; }
        private Func<IPlayerConfigurationViewModel> FakePlayerConfigurationFactory { get; set; }
        private IPlayerConfigurationViewModel FakePlayerConfigurationViewModel { get; set; }
        private Func<ICollection<IPlayer>, IGame> FakeGameFactory { get; set; }
        private Func<IRoundResult, IRoundResultViewModel> FakeRoundResultFactory { get; set; }

        /// <summary>
        /// Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            FakeGameController = Substitute.For<IGameController>();
            FakeUiVisualizerService = Substitute.For<IUIVisualizerService>();
            FakePlayerConfigurationFactory = Substitute.For<Func<IPlayerConfigurationViewModel>>();
            FakePlayerConfigurationViewModel = Substitute.For<IPlayerConfigurationViewModel>();
            FakeGameFactory = Substitute.For<Func<ICollection<IPlayer>, IGame>>();
            FakeRoundResultFactory = Substitute.For<Func<IRoundResult, IRoundResultViewModel>>();
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
        /// Test for the ConfigurePlayersCommand when CanExecute.
        /// </summary>
        [TestMethod]
        public void ConfigurePlayersCommand_CanExecute_GameNotStarted_ReturnsTrue()
        {
            // ARRANGE
            FakeGameController.IsGameStarted.Returns(false);

            // ACT
            var actual = Target.ConfigurePlayersCommand.CanExecute();

            // ASSERT
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Test for the ConfigurePlayersCommand when CanExecute.
        /// </summary>
        [TestMethod]
        public void ConfigurePlayersCommand_CanExecute_GameStarted_ReturnsFalse()
        {
            // ARRANGE
            FakeGameController.IsGameStarted.Returns(true);

            // ACT
            var actual = Target.ConfigurePlayersCommand.CanExecute();

            // ASSERT
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Test for the ConfigurePlayersCommand when ShowsSettingsDialog.
        /// </summary>
        [TestMethod]
        public void ConfigurePlayersCommand_Execute_ShowsSettingsDialog()
        {
            // ARRANGE
            FakePlayerConfigurationFactory.Invoke().Returns(FakePlayerConfigurationViewModel);

            // ACT
            Target.ConfigurePlayersCommand.Execute();

            // ASSERT
            FakeUiVisualizerService.Received().ShowDialog(FakePlayerConfigurationViewModel);
        }

        /// <summary>
        /// Test for the ConfigurePlayersCommand when ShowsSettingsDialog.
        /// </summary>
        [TestMethod]
        public void ConfigurePlayersCommand_Execute_DialogReturnsTrue_SetsPlayersAndStartsNewGame()
        {
            // ARRANGE
            FakePlayerConfigurationViewModel.Players.Returns(
                new System.Collections.ObjectModel.ObservableCollection<IPlayer>
                    {
                        Substitute.For<IPlayer>(),
                        Substitute.For<IPlayer>()
                    });

            FakePlayerConfigurationFactory.Invoke().Returns(FakePlayerConfigurationViewModel);
            FakeUiVisualizerService.ShowDialog(FakePlayerConfigurationViewModel).Returns(true);

            var fakeGame = Substitute.For<IGame>();
            FakeGameFactory.Invoke(FakePlayerConfigurationViewModel.Players).Returns(fakeGame);

            // ACT
            Target.ConfigurePlayersCommand.Execute();

            // ASSERT
            Assert.AreEqual(FakePlayerConfigurationViewModel.Players, FakeGameController.Players);
            FakeGameController.Received().StartNewGame(fakeGame);
        }

        /// <summary>
        /// Test for the ConfigurePlayersCommand when Execute.
        /// </summary>
        [TestMethod]
        public void ConfigurePlayersCommand_Execute_DialogReturnsFalse_DoesNotSetPlayersOrStartNewGame()
        {
            // ARRANGE
            FakePlayerConfigurationViewModel.Players.Returns(
                new System.Collections.ObjectModel.ObservableCollection<IPlayer>
                    {
                        Substitute.For<IPlayer>(),
                        Substitute.For<IPlayer>()
                    });

            FakePlayerConfigurationFactory.Invoke().Returns(FakePlayerConfigurationViewModel);
            FakeUiVisualizerService.ShowDialog(FakePlayerConfigurationViewModel).Returns(false);

            var fakeGame = Substitute.For<IGame>();
            FakeGameFactory.Invoke(FakePlayerConfigurationViewModel.Players).Returns(fakeGame);

            // ACT
            Target.ConfigurePlayersCommand.Execute();

            // ASSERT
            FakeGameController.DidNotReceive().Players = FakePlayerConfigurationViewModel.Players;
            FakeGameController.DidNotReceive().StartNewGame(fakeGame);
        }

        /// <summary>
        /// Test for the StartRoundCommand when CanExecute.
        /// </summary>
        [TestMethod]
        public void StartRoundCommand_CanExecute_GameNotStarted_ReturnsFalse()
        {
            // ARRANGE
            FakeGameController.IsGameStarted.Returns(false);

            // ACT
            var actual = Target.StartRoundCommand.CanExecute();

            // ASSERT
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Test for the ConfigurePlayersCommand when CanExecute.
        /// </summary>
        [TestMethod]
        public void StartRoundCommand_CanExecute_GameStarted_NotPlaying_ReturnsTrue()
        {
            // ARRANGE
            FakeGameController.IsGameStarted.Returns(true);
            FakeGameController.IsPlaying.Returns(false);

            // ACT
            var actual = Target.StartRoundCommand.CanExecute();

            // ASSERT
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Test for the ConfigurePlayersCommand when CanExecute.
        /// </summary>
        [TestMethod]
        public void StartRoundCommand_CanExecute_GameStarted_IsPlaying_ReturnsFalse()
        {
            // ARRANGE
            FakeGameController.IsGameStarted.Returns(true);
            FakeGameController.IsPlaying.Returns(true);

            // ACT
            var actual = Target.StartRoundCommand.CanExecute();

            // ASSERT
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Test for the StartRoundCOmmand when Execute.
        /// </summary>
        [TestMethod]
        public void StartRoundCOmmand_Execute_CallsStartOnGameController()
        {
            // ARRANGE
            FakeGameController.IsGameStarted.Returns(true);
            FakeGameController.IsPlaying.Returns(false);

            // ACT
            Target.StartRoundCommand.Execute();

            // ASSERT
            FakeGameController.Received().StartRound();
        }

        /// <summary>
        /// Test for the EndRoundCommand when CanExecute.
        /// </summary>
        [TestMethod]
        public void EndRoundCommand_CanExecute_GameNotStarted_ReturnsFalse()
        {
            // ARRANGE
            FakeGameController.IsGameStarted.Returns(false);

            // ACT
            var actual = Target.EndRoundCommand.CanExecute();

            // ASSERT
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Test for the EndRoundCommand when CanExecute.
        /// </summary>
        [TestMethod]
        public void EndRoundCommand_CanExecute_GameStarted_NotPlaying_ReturnsFalse()
        {
            // ARRANGE
            FakeGameController.IsGameStarted.Returns(true);
            FakeGameController.IsPlaying.Returns(false);

            // ACT
            var actual = Target.EndRoundCommand.CanExecute();

            // ASSERT
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Test for the EndRoundCommand when CanExecute.
        /// </summary>
        [TestMethod]
        public void EndRoundCommand_CanExecute_GameStarted_IsPlaying_ReturnsTrue()
        {
            // ARRANGE
            FakeGameController.IsGameStarted.Returns(true);
            FakeGameController.IsPlaying.Returns(true);

            // ACT
            var actual = Target.EndRoundCommand.CanExecute();

            // ASSERT
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Test for the EndRoundCommand when Execute.
        /// </summary>
        [TestMethod]
        public void EndRoundCommand_Execute_CallsEndRoundOnGameController()
        {
            // ARRANGE
            FakeGameController.IsGameStarted.Returns(true);
            FakeGameController.IsPlaying.Returns(true);
            //FakeGameController.EndRound().Returns(Substitute.For<IRoundResult>());

            // ACT
            Target.EndRoundCommand.Execute();

            // ASSERT
            FakeGameController.Received().EndRound();
        }

        /// <summary>
        /// Test for the EndRoundCommand when Execute.
        /// </summary>
        [TestMethod]
        public void EndRoundCommand_Execute_ShowsResultView()
        {
            // ARRANGE
            FakeGameController.IsGameStarted.Returns(true);
            FakeGameController.IsPlaying.Returns(true);

            var roundResult = Substitute.For<IRoundResult>();
            FakeGameController.EndRound().Returns(roundResult);
            var viewModel = Substitute.For<IRoundResultViewModel>();
            FakeRoundResultFactory.Invoke(roundResult).Returns(viewModel);

            // ACT
            Target.EndRoundCommand.Execute();

            // ASSERT
            FakeUiVisualizerService.Received().ShowDialog(viewModel);
        }
    }
}