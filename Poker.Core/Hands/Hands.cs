using System.ComponentModel;

namespace Poker.Core.Hands
{
    /// <summary>
    /// The possible rankings of a hand, according to pre-determined rules.
    /// The higher the number the better the hand.
    /// </summary>
    public enum Hands
    {
        [Description("High Card")]
        HighCard,
        [Description("One Pair")]
        OnePair,
        [Description("Two Pair")]
        TwoPair,
        [Description("Three of a Kind")]
        ThreeOfAKind,
        Straight,
        Flush,
        [Description("Full House")]
        FullHouse,
        [Description("Four of a Kind")]
        FourOfAKind,
        [Description("Straight Flush")]
        StraightFlush
    }
}