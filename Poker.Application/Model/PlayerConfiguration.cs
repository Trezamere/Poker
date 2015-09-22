using System.Collections.ObjectModel;
using Catel.Data;
using Catel.IoC;
using Poker.Core;

namespace Poker.Application.Model
{
    /// <summary>
    /// Gets the player configuration settings.
    /// </summary>
    [ServiceLocatorRegistration(typeof(IPlayerConfiguration), ServiceLocatorRegistrationMode.Transient)]
    public class PlayerConfiguration : SavableModelBase<IPlayerConfiguration>, IPlayerConfiguration
    {
        #region Players property

        /// <summary>
        /// Identifies the <see cref="Players"/> property.
        /// </summary>
        public static readonly PropertyData PlayersProperty = RegisterProperty("Players",
            typeof (ObservableCollection<IPlayer>),
            new ObservableCollection<IPlayer> {new Player {Name = "Player 1"}, new Player {Name = "Player 2"}});

        /// <summary>
        /// Gets or sets the collection of players
        /// </summary>
        public ObservableCollection<IPlayer> Players
        {
            get { return GetValue<ObservableCollection<IPlayer>>(PlayersProperty); }
            set { SetValue(PlayersProperty, value); }
        }

        #endregion
    }
}