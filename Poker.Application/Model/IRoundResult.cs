using System.Collections.ObjectModel;
using Catel.Data;
using Poker.Core;

namespace Poker.Application.Model
{
    /// <summary>
    /// Defines the results for a round of poker.
    /// </summary>
    public interface IRoundResult : IModel
    {
        /// <summary>
        /// Gets or sets the collection of Winners
        /// </summary>
        ObservableCollection<IPlayer> Winners { get; }

        /// <summary>
        /// Gets or sets the collection of Winners
        /// </summary>
        ObservableCollection<IPlayer> Losers { get; }
    }
}