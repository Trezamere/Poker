using System.Collections.ObjectModel;
using Catel.MVVM;
using Poker.Core;

namespace Poker.Application.ViewModel
{
    /// <summary>
    /// Defines the methods and properties for the main view model.
    /// </summary>
    public interface IMainWindowViewModel : IViewModel
    {
        /// <summary>
        /// Gets the command to configure the players.
        /// </summary>
        Command ConfigurePlayersCommand { get; }

        /// <summary>
        /// Gets the command to Start the game.
        /// </summary>
        Command StartRoundCommand { get; }

        /// <summary>
        /// Gets the command to End the game.
        /// </summary>
        Command EndRoundCommand { get; }

        /// <summary>
        /// Gets or sets the collection of players
        /// </summary>
        ObservableCollection<IPlayer> Players { get; set; }
    }
}