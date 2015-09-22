using System;
using System.Collections.Generic;
using System.Linq;
using Catel;
using Poker.Core.Deck;
using Poker.Core.Hands;

namespace Poker.Core
{
    /// <summary>
    /// Handles the basic logic for playing a game of poker.
    /// </summary>
    public class Game : IGame
    {
        /// <summary>
        /// The deck of cards necessary to play a game.
        /// </summary>
        private readonly IDeck _deck;

        /// <summary>
        /// The collection of players playing the game.
        /// </summary>
        private readonly List<IPlayer> _players;

        /// <summary>
        /// Comparer responsible for comparing players hands.
        /// </summary>
        private readonly IComparer<IPlayer> _playerHandComparer;

        /// <summary>
        /// The factory that handles creating hands from a list of cards.
        /// </summary>
        private readonly Func<List<Card>, IHand> _handFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="deck">The deck of cards necessary to play a game.</param>
        /// <param name="players">The players playing the game.</param>
        /// <param name="playerHandComparer">Comparer that will compare the hands of two players.</param>
        /// <param name="handFactory">The factory that handles creating hands from a list of cards.</param>
        public Game(IDeck deck,
            ICollection<IPlayer> players,
            IComparer<IPlayer> playerHandComparer,
            Func<List<Card>, IHand> handFactory)
        {
            Argument.IsNotNull(() => deck);
            Argument.IsNotNull(() => players);
            Argument.IsNotNull(() => playerHandComparer);
            Argument.IsNotNull(() => handFactory);
            if (players.Count < 2)
                throw new ArgumentException("A game of poker requires at least two players to play.");

            _deck = deck;
            _playerHandComparer = playerHandComparer;
            _players = players.ToList();
            _handFactory = handFactory;
        }

        /// <summary>
        /// Starts a new game.  Shuffles the deck and deals cards to each player.
        /// </summary>
        public void Start()
        {
            _deck.Shuffle();

            // Deal five cards to each player.
            var hands = new List<Card>[_players.Count];
            for (int i = 0; i < 5; i++)
            {
                // Deal to each hand in a rotation.
                for (int j = 0; j < _players.Count; j++)
                {
                    // Initialize the list on the first pass
                    if (i == 0)
                        hands[j] = new List<Card>();

                    hands[j].Add(_deck.Draw());
                }
            }

            // Deal the hands to the players.
            int k = 0;
            foreach (var player in _players)
            {
                player.Hand = _handFactory(hands[k]);
                k++;
            }
        }

        /// <summary>
        /// Ends the game and returns the winner of the pot.
        /// <para>There may be more than one winner in the case of a tie.</para>
        /// </summary>
        /// <returns>the collection of winners for this game.</returns>
        public ICollection<IPlayer> End()
        {
            // Sort the players by hand.
            _players.Sort(_playerHandComparer);

            // The first player will be the winner.
            var winners = new List<IPlayer> {_players[0]};

            // Check for ties.
            for (int i = 1; i < _players.Count; i++)
            {
                // If the player hands match, we have a tie.
                if (_players[i].Hand.HighCard == _players[0].Hand.HighCard &&
                    _players[i].Hand.Type == _players[0].Hand.Type)
                    winners.Add(_players[i]);
                else
                    // Can stop checking since the list is sorted, if there was not a match.
                    break;
            }

            return winners;
        }
    }
}