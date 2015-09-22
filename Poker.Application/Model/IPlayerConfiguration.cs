using System.Collections.ObjectModel;
using Catel.Data;
using Poker.Core;

namespace Poker.Application.Model
{
    /// <summary>
    /// Defines the methods and properties for player configuration.
    /// </summary>
    public interface IPlayerConfiguration : ISavableModel
    {
        /// <summary>
        /// Gets or sets the collection of players
        /// </summary>
        ObservableCollection<IPlayer> Players { get; set; }
    }
}