using DuelMastersInterfaceModels.Cards;
using System.Collections.Generic;

namespace DuelMastersModels.Cards
{
    /// <summary>
    /// Interface for Duel Masters cards.
    /// </summary>
    public interface ICard
    {
        /// <summary>
        /// An unique identifier for the card during a duel.
        /// </summary>
        int GameID { get; }

        IPlayer Owner { get; set; }

        /// <summary>
        /// Civilizations of the card.
        /// </summary>
        IEnumerable<Civilization> Civilizations { get; }

        /// <summary>
        /// Mana cost of the card.
        /// </summary>
        int Cost { get; }

        CardIdentifier CardID { get; }

        bool Tapped { get; set; }
    }
}
