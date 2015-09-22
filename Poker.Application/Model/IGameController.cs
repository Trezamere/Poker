using System.Collections.ObjectModel;
using Poker.Core;

namespace Poker.Application.Model
{
    /// <summary>
    /// Defines the methods and properties which handle the user interaction logic for playing a game.
    /// </summary>
    public interface IGameController
    {
        /// <summary>
        /// Gets or sets the collection of players
        /// </summary>
        bool IsPlaying { get; set; }

        /// <summary>
        /// Gets or sets the collection of players
        /// </summary>
        ObservableCollection<IPlayer> Players { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if a game type has been started.
        /// </summary>
        bool IsGameStarted { get; set; }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        void StartRound();

        /// <summary>
        /// Ends the game and returns the winner of the pot. 
        /// <para>There may be more than one winner in the case of a tie.</para>
        /// </summary>
        /// <returns>the result of the round.</returns>
        IRoundResult EndRound();

        /// <summary>
        /// Starts a new game type.
        /// </summary>
        /// <param name="game">The new game type to start playing.</param>
        void StartNewGame(IGame game);
    }
}