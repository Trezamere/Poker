using System.Collections.Generic;
using System.Collections.ObjectModel;
using Catel.Data;
using Catel.IoC;
using Poker.Core;

namespace Poker.Application.Model
{
    /// <summary>
    /// Contains the results of a round of poker.
    /// </summary>
    [ServiceLocatorRegistration(typeof(IRoundResult), ServiceLocatorRegistrationMode.Transient)]
    public class RoundResult : ModelBase, IRoundResult
    {
        #region Winners property

        /// <summary>
        /// Identifies the <see cref="Winners"/> property.
        /// </summary>
        public static readonly PropertyData WinnersProperty = RegisterProperty("Winners",
            typeof(ObservableCollection<IPlayer>));

        /// <summary>
        /// Gets or sets the collection of Winners
        /// </summary>
        public ObservableCollection<IPlayer> Winners
        {
            get { return GetValue<ObservableCollection<IPlayer>>(WinnersProperty); }
            private set { SetValue(WinnersProperty, value); }
        }

        #endregion

        #region Losers property

        /// <summary>
        /// Identifies the <see cref="Losers"/> property.
        /// </summary>
        public static readonly PropertyData LosersProperty = RegisterProperty("Losers",
            typeof(ObservableCollection<IPlayer>));

        /// <summary>
        /// Gets or sets the collection of Winners
        /// </summary>
        public ObservableCollection<IPlayer> Losers
        {
            get { return GetValue<ObservableCollection<IPlayer>>(LosersProperty); }
            private set { SetValue(LosersProperty, value); }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundResult"/> class.
        /// </summary>
        /// <param name="winners">The collection of winners for a round.</param>
        /// <param name="losers">The collection of losers for a round.</param>
        public RoundResult(IEnumerable<IPlayer> winners, IEnumerable<IPlayer> losers)
        {
            Winners = new ObservableCollection<IPlayer>(winners);
            Losers = new ObservableCollection<IPlayer>(losers);
        }
    }
}