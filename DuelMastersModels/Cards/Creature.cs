using DuelMastersModels.Abilities.TriggeredAbilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DuelMastersModels.Cards
{
    public enum Race
    {
        LiquidPeople,
        BeastFolk,
    }

    /// <summary>
    /// Creature is a card type.
    /// </summary>
    public abstract class Creature : Card, ICreature
    {
        /// <summary>
        /// The base power of creature. Use Duel's method GetPower(creature) in order to get the actual power of a creature.
        /// </summary>
        public int Power { get; private set; }

        /// <summary>
        /// Race is a characteristic of a creature.
        /// </summary>
        public ICollection<Race> Races { get; }

        public ICollection<ITriggeredAbility> TriggerAbilities { get; }

        /// <summary>
        /// Creates a creature.
        /// </summary>
        protected Creature(int cost, Civilization civilization, int power, Race race) : base(cost, civilization)
        {
            Power = power;
            Races = new Collection<Race> { race };
        }
    }
}
