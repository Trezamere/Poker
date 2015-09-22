using Catel.Data;
using Poker.Core.Hands;

namespace Poker.Core
{
    /// <summary>
    /// A player.
    /// </summary>
    public interface IPlayer : IModel
    {
        /// <summary>
        /// Gets or sets the name of the player..
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The players hand.
        /// </summary>
        IHand Hand { get; set; }
    }
}