using System.Collections.Generic;
using System.Xml.Serialization;

namespace DuelMastersInterfaceModels.Cards
{
    /// <summary>
    /// Represents the civilizations cards can have.
    /// </summary>
    public enum Civilization
    {
        /// <summary>
        /// Light civilization. (white/yellow)
        /// </summary>
        Light,

        /// <summary>
        /// Water civilization. (blue)
        /// </summary>
        Water,

        /// <summary>
        /// Darkness civilization. (black)
        /// </summary>
        Darkness,

        /// <summary>
        /// Fire civilization. (red)
        /// </summary>
        Fire,

        /// <summary>
        /// Nature civilization. (green)
        /// </summary>
        Nature
    };

    [XmlInclude(typeof(CreatureWrapper))]
    public abstract class CardWrapper
    {
        public int GameID { get; set; }

        public List<Civilization> Civilizations { get; set; }

        /// <summary>
        /// Mana cost of the card.
        /// </summary>
        public int Cost { get; set; }

        public CardIdentifier CardID { get; set; }
    }
}
