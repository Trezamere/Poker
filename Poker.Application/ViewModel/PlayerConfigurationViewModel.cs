using System.Collections.ObjectModel;
using Catel;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Poker.Application.Model;
using Poker.Core;

namespace Poker.Application.ViewModel
{
    /// <summary>
    /// ViewModel for the player configuration settings.
    /// </summary>
    [ServiceLocatorRegistration(typeof(IPlayerConfigurationViewModel), ServiceLocatorRegistrationMode.Transient)]
    public class PlayerConfigurationViewModel : ViewModelBase, IPlayerConfigurationViewModel
    {
        #region Configuration property

        /// <summary>
        /// Identifies the <see cref="Configuration"/> property.
        /// </summary>
        public static readonly PropertyData ConfigurationProperty = RegisterProperty("Configuration",
            typeof (IPlayerConfiguration));

        /// <summary>
        /// Gets or sets the player configuration.
        /// </summary>
        [Model]
        public IPlayerConfiguration Configuration
        {
            get { return GetValue<IPlayerConfiguration>(ConfigurationProperty); }
            set { SetValue(ConfigurationProperty, value); }
        }

        #endregion

        #region Players property

        /// <summary>
        /// Identifies the <see cref="Players"/> property.
        /// </summary>
        public static readonly PropertyData PlayersProperty = RegisterProperty("Players",
            typeof (ObservableCollection<IPlayer>));

        /// <summary>
        /// Gets or sets the collection of players
        /// </summary>
        [ViewModelToModel("Configuration")]
        public ObservableCollection<IPlayer> Players
        {
            get { return GetValue<ObservableCollection<IPlayer>>(PlayersProperty); }
            set { SetValue(PlayersProperty, value); }
        }

        #endregion

        #region SelectedPlayer property

        /// <summary>
        /// Identifies the <see cref="SelectedPlayer"/> property.
        /// </summary>
        public static readonly PropertyData SelectedPlayerProperty = RegisterProperty("SelectedPlayer", typeof (IPlayer));

        /// <summary>
        /// Gets or sets the collection of players
        /// </summary>
        public IPlayer SelectedPlayer
        {
            get { return GetValue<IPlayer>(SelectedPlayerProperty); }
            set { SetValue(SelectedPlayerProperty, value); }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerConfigurationViewModel"/> class.
        /// </summary>
        public PlayerConfigurationViewModel(IPlayerConfiguration playerConfiguration)
        {
            Argument.IsNotNull(() => playerConfiguration);

            Configuration = playerConfiguration;
        }
    }
}