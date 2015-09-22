using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Catel;
using Catel.Data;
using Catel.IoC;
using Poker.Core;

namespace Poker.Application.Model
{
    /// <summary>
    /// Handles the user interaction logic for playing a game.
    /// </summary>
    [ServiceLocatorRegistration(typeof(IGameController), ServiceLocatorRegistrationMode.Transient)]
    public class GameController : ModelBase, IGameController
    {
        /// <summary>
        /// Factory for creating <see cref="IRoundResult"/> objects.
        /// </summary>
        private readonly Func<IEnumerable<IPlayer>, IEnumerable<IPlayer>, IRoundResult> _roundResultFactory;

        /// <summary>
        /// The current game type.
        /// </summary>
        private IGame _currentGame;

        #region IsGameStarted property

        /// <summary>
        /// Identifies the <see cref="IsGameStarted"/> property.
        /// </summary>
        public static readonly PropertyData IsGameStartedProperty = RegisterProperty("IsGameStarted", typeof(bool));

        /// <summary>
        /// Gets or sets a flag indicating if a game type has been started.
        /// </summary>
        public bool IsGameStarted
        {
            get { return GetValue<bool>(IsGameStartedProperty); }
            set { SetValue(IsGameStartedProperty, value); }
        }

        #endregion

        #region IsPlaying property

        /// <summary>
        /// Identifies the <see cref="IsPlaying"/> property.
        /// </summary>
        public static readonly PropertyData IsPlayingProperty = RegisterProperty("IsPlaying", typeof(bool));

        /// <summary>
        /// Gets or sets a flag indicating if a round is currently playing.
        /// </summary>
        public bool IsPlaying
        {
            get { return GetValue<bool>(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
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
        public ObservableCollection<IPlayer> Players
        {
            get { return GetValue<ObservableCollection<IPlayer>>(PlayersProperty); }
            set { SetValue(PlayersProperty, value); }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GameController"/> class.
        /// </summary>
        public GameController(Func<IEnumerable<IPlayer>, IEnumerable<IPlayer>, IRoundResult> roundResultFactory)
        {
            Argument.IsNotNull(() => roundResultFactory);

            _roundResultFactory = roundResultFactory;
        }

        /// <summary>
        /// Starts a new game type.
        /// </summary>
        /// <param name="game">The new game type to start playing.</param>
        public void StartNewGame(IGame game)
        {
            Argument.IsNotNull(() => game);

            _currentGame = game;

            IsGameStarted = true;
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        public void StartRound()
        {
            if (_currentGame != null)
            {
                _currentGame.Start();
                IsPlaying = true;
            }
        }

        /// <summary>
        /// Ends the game and returns the winner of the pot. 
        /// <para>There may be more than one winner in the case of a tie.</para>
        /// </summary>
        /// <returns>the collection of winners for this game.</returns>
        public IRoundResult EndRound()
        {
            if (IsPlaying)
            {
                IsPlaying = false;
                var winners = _currentGame.End();
                var losers = Players.Except(winners);

                return _roundResultFactory(winners, losers);
            }

            return _roundResultFactory(new List<IPlayer>(0), new List<IPlayer>(0));
        }
    }
}