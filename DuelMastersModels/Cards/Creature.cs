using DuelMastersInterfaceModels.Cards;
using DuelMastersModels.Abilities.TriggeredAbilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DuelMastersModels.Cards
{
    /// <summary>
    /// Creature is a card type.
    /// </summary>
    public abstract class Creature : Card
    {
        /// <summary>
        /// The base power of creature. Use Duel's method GetPower(creature) in order to get the actual power of a creature.
        /// </summary>
        public int Power { get; private set; }

        /// <summary>
        /// Race is a characteristic of a creature.
        /// </summary>
        public IEnumerable<Race> Races { get; } = new Collection<Race>();

        public ICollection<TriggeredAbility> TriggerAbilities { get; }
        public bool SummoningSickness { get; } = true;

        /// <summary>
        /// Creates a creature.
        /// </summary>
        protected Creature(CardIdentifier cardID, int cost, Civilization civilization, int power, Race race) : base(cardID, cost, civilization)
        {
            Power = power;
            Races = new Collection<Race> { race };
        }
    }
}
