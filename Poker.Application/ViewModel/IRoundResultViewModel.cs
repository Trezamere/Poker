using System.Collections.ObjectModel;
using Catel.MVVM;
using Poker.Core;

namespace Poker.Application.ViewModel
{
    /// <summary>
    /// Defines the properties for a ViewModel for a result of a round of poker.
    /// </summary>
    public interface IRoundResultViewModel : IViewModel
    {
        /// <summary>
        /// Gets or sets the collection of winners.
        /// </summary>
        ObservableCollection<IPlayer> Winners { get; }

        /// <summary>
        /// Gets or sets the collection of losers.
        /// </summary>
        ObservableCollection<IPlayer> Losers { get; }
    }
}