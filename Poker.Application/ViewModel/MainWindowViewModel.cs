using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Catel;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using Poker.Application.Model;
using Poker.Core;
using System.Linq;

namespace Poker.Application.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    [ServiceLocatorRegistration(typeof(IMainWindowViewModel), ServiceLocatorRegistrationMode.Transient)]
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        /// <summary>
        /// Service which handles displaying dialogs.
        /// </summary>
        private readonly IUIVisualizerService _uiVisualizerService;

        /// <summary>
        /// Factory which creates player configuration view models.
        /// </summary>
        private readonly Func<IPlayerConfigurationViewModel> _playerConfigurationFactory;

        /// <summary>
        /// Factory which is responsible for creating game types.
        /// </summary>
        private readonly Func<ICollection<IPlayer>, IGame> _gameFactory;

        /// <summary>
        /// Factory which creates round result view models.
        /// </summary>
        private readonly Func<IRoundResult, IRoundResultViewModel> _roundResultFactory;

        #region GameController Property

        /// <summary>
        /// Identifies the <see cref="GameController"/> property.
        /// </summary>
        public static readonly PropertyData GameControllerProperty = RegisterProperty("GameController", typeof (IGameController));

        /// <summary>
        /// Gets or sets the game controller.
        /// </summary>
        [Model]
        public IGameController GameController
        {
            get { return GetValue<IGameController>(GameControllerProperty); }
            set { SetValue(GameControllerProperty, value); }
        }

        #endregion

        #region Players property

        /// <summary>
        /// Identifies the <see cref="Players"/> property.
        /// </summary>
        public static readonly PropertyData PlayersProperty = RegisterProperty("Players",
            typeof(ObservableCollection<IPlayer>),
            new ObservableCollection<IPlayer>());

        /// <summary>
        /// Gets or sets the collection of players
        /// </summary>
        [ViewModelToModel("GameController")]
        public ObservableCollection<IPlayer> Players
        {
            get { return GetValue<ObservableCollection<IPlayer>>(PlayersProperty); }
            set { SetValue(PlayersProperty, value); }
        }

        #endregion

        /// <summary>
        /// Gets the command to configure the players.
        /// </summary>
        public Command ConfigurePlayersCommand { get; private set; }

        /// <summary>
        /// Gets the command to Start the game.
        /// </summary>
        public Command StartRoundCommand { get; private set; }

        /// <summary>
        /// Gets the command to End the game.
        /// </summary>
        public Command EndRoundCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainWindowViewModel(
            IGameController gameController,
            IUIVisualizerService uiVisualizerService,
            Func<IPlayerConfigurationViewModel> playerConfigurationFactory,
            Func<ICollection<IPlayer>, IGame> gameFactory,
            Func<IRoundResult, IRoundResultViewModel> roundResultFactory)
        {
            Argument.IsNotNull(() => gameController);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => playerConfigurationFactory);
            Argument.IsNotNull(() => gameFactory);
            Argument.IsNotNull(() => roundResultFactory);

            GameController = gameController;
            _uiVisualizerService = uiVisualizerService;
            _playerConfigurationFactory = playerConfigurationFactory;
            _gameFactory = gameFactory;
            _roundResultFactory = roundResultFactory;

            ConfigurePlayersCommand = new Command(OnConfigurePlayers, () => !GameController.IsGameStarted);
            StartRoundCommand = new Command(OnStartRound, () => GameController.IsGameStarted && !GameController.IsPlaying);
            EndRoundCommand = new Command(OnEndRound, () => GameController.IsGameStarted && GameController.IsPlaying);
        }

        /// <summary>
        /// Shows the player configuration view when the <see cref="ConfigurePlayersCommand"/> is executed.
        /// </summary>
        private void OnConfigurePlayers()
        {
            var playerSettings = _playerConfigurationFactory();
            // If the user doesn't cancel the view and there are no errors, start the game.
            if (_uiVisualizerService.ShowDialog(playerSettings) == true &&
                playerSettings.Players.All(t => !t.HasErrors))
            {
                GameController.Players = playerSettings.Players;
                GameController.StartNewGame(_gameFactory(playerSettings.Players));

                StartRoundCommand.RaiseCanExecuteChanged();
                EndRoundCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Starts a new game when the <see cref="StartRoundCommand"/> is executed.
        /// </summary>
        /// <returns></returns>
        private void OnStartRound()
        {
            GameController.StartRound();

            StartRoundCommand.RaiseCanExecuteChanged();
            EndRoundCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Starts a new game when the <see cref="EndRoundCommand"/> is executed.
        /// </summary>
        /// <returns></returns>
        private void OnEndRound()
        {
            var result = GameController.EndRound();

            var viewModel = _roundResultFactory(result);
            _uiVisualizerService.ShowDialog(viewModel);

            StartRoundCommand.RaiseCanExecuteChanged();
            EndRoundCommand.RaiseCanExecuteChanged();
        }
    }
}