using Catel.Data;
using Catel.IoC;
using Poker.Core.Hands;

namespace Poker.Core
{
    /// <summary>
    /// A player.
    /// </summary>
    [ServiceLocatorRegistration(typeof(IPlayer), ServiceLocatorRegistrationMode.Transient)]
    public class Player : ModelBase, IPlayer
    {
        #region Name property

        /// <summary>
        /// Identifies the <see cref="Name"/> property.
        /// </summary>
        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof (string));

        /// <summary>
        /// Gets or sets the name of the player..
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        #endregion
        
        #region Hand property

        /// <summary>
        /// Identifies the <see cref="Hand"/> property.
        /// </summary>
        public static readonly PropertyData HandProperty = RegisterProperty("Hand", typeof (IHand));

        /// <summary>
        /// The players hand.
        /// </summary>
        public IHand Hand
        {
            get { return GetValue<IHand>(HandProperty); }
            set { SetValue(HandProperty, value); }
        }

        #endregion
    }
}