using System.Collections.ObjectModel;
using Catel.MVVM;
using Poker.Core;

namespace Poker.Application.ViewModel
{
    /// <summary>
    /// Defines the properties and methods for the player configuration view model.
    /// </summary>
    public interface IPlayerConfigurationViewModel : IViewModel
    {
        /// <summary>
        /// Gets or sets the collection of players
        /// </summary>
        ObservableCollection<IPlayer> Players { get; set; }

        /// <summary>
        /// Gets or sets the collection of players
        /// </summary>
        IPlayer SelectedPlayer { get; set; }
    }
}