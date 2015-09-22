using System.Collections.Generic;

namespace Poker.Core
{
    /// <summary>
    /// Defines the methods handling the basic logic for playing a game of poker.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Starts a new game.  Shuffles the deck and deals cards to each player.
        /// </summary>
        void Start();

        /// <summary>
        /// Ends the game and returns the winner of the pot.
        /// <para>There may be more than one winner in the case of a tie.</para>
        /// </summary>
        /// <returns>the collection of winners for this game.</returns>
        ICollection<IPlayer> End();
    }
}