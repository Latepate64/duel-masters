using DuelMastersInterfaceModels.Cards;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DuelMastersModels.Cards
{
    /// <summary>
    /// Represent a Duel Masters card.
    /// </summary>
    public abstract class Card : ICard
    {
        /// <summary>
        /// An unique identifier for the card during a duel.
        /// </summary>
        public int GameID { get; }

        public IPlayer Owner { get; set; }

        /// <summary>
        /// Civilizations of the card.
        /// </summary>
        public IEnumerable<Civilization> Civilizations { get; }

        public CardIdentifier CardID { get; }

        /// <summary>
        /// Mana cost of the card.
        /// </summary>
        public int Cost { get; private set; }

        public bool Tapped { get; set; }

        /// <summary>
        /// Creates a card.
        /// </summary>
        protected Card(CardIdentifier cardID, int cost, IEnumerable<Civilization> civilizations)
        {
            GameID = _gameID++;
            CardID = cardID;
            Civilizations = civilizations;
            Cost = cost;
        }

        /// <summary>
        /// Creates a card.
        /// </summary>
        protected Card(CardIdentifier cardID, int cost, Civilization civilization) : this(cardID, cost, new Collection<Civilization> { civilization }) { }

        private static int _gameID = 0;
    }
}
