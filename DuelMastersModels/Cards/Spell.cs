using DuelMastersInterfaceModels.Cards;

namespace DuelMastersModels.Cards
{
    /// <summary>
    /// Spell is a card type.
    /// </summary>
    public abstract class Spell : Card
    {
        /// <summary>
        /// Creates a spell.
        /// </summary>
        protected Spell(CardIdentifier cardID, int cost, Civilization civilization) : base(cardID, cost, civilization) { }
    }
}
