using System.Collections.Generic;

namespace DuelMastersInterfaceModels.Cards
{
    public interface ICard
    {
        /// <summary>
        /// An unique identifier for the card during a duel.
        /// </summary>
        int GameID { get; }

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
