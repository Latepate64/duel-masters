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
        public int ID { get; }

        public IPlayer Owner { get; set; }

        /// <summary>
        /// Civilizations of the card.
        /// </summary>
        public IEnumerable<Civilization> Civilizations { get; }

        /// <summary>
        /// Mana cost of the card.
        /// </summary>
        public int Cost { get; private set; }

        /// <summary>
        /// Creates a card.
        /// </summary>
        protected Card(int cost, IEnumerable<Civilization> civilizations)
        {
            Civilizations = civilizations;
            Cost = cost;
        }

        /// <summary>
        /// Creates a card.
        /// </summary>
        protected Card(int cost, Civilization civilization) : this(cost, new Collection<Civilization> { civilization }) { }
    }
}
