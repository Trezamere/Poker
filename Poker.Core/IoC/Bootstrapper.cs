using System;
using System.Collections.Generic;
using Catel.IoC;
using Poker.Core.Deck;
using Poker.Core.Hands;
using Poker.Core.Hands.Comparers;

namespace Poker.Core.IoC
{
    /// <summary>
    /// Bootstrapper for this assembly which is used to configure the IoC container.
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        /// Configures the specified IoC container with the type resolutions from this library.
        /// </summary>
        public void Configure(IServiceLocator container)
        {
            var cardComparer = new CardComparer();
            container.RegisterInstance<IComparer<Card>>(cardComparer);

            var highCardComparer = new HighCardComparer(cardComparer);
            var handComparer = new HandComparer(
                highCardComparer,
                new PairComparer(highCardComparer),
                new TwoPairComparer(),
                new ThreeOfAKindComparer(highCardComparer),
                new StraightComparer(),
                new FlushComparer(highCardComparer),
                new FullHouseComparer(highCardComparer),
                new FourOfAKindComparer(highCardComparer),
                new StraightFlushComparer());
            container.RegisterInstance<IHandComparer>(handComparer);

            container.RegisterInstance<Func<List<Card>, IHand>>(
                cards => new Hand(
                    cards,
                    cardComparer,
                    handComparer));

            container.RegisterInstance<Func<ICollection<IPlayer>, IGame>>(
                players => new Game(
                    container.ResolveType<IDeckFactory>().CreateStandardDeck(),
                    players,
                    container.ResolveType<IComparer<IPlayer>>(),
                    container.ResolveType<Func<List<Card>, IHand>>()));
        } 
    }
}