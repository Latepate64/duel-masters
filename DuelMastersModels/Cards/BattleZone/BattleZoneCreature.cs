using DuelMastersInterfaceModels.Cards;
using DuelMastersModels.Abilities.TriggeredAbilities;
using System.Collections.Generic;

namespace DuelMastersModels.Cards
{
    /// <summary>
    /// Represents a creature that exists in battle zone.
    /// </summary>
    public class BattleZoneCreature : BattleZoneCard, IBattleZoneCreature
    {
        /// <summary>
        /// Power of the creature.
        /// </summary>
        public int Power { get; }

        /// <summary>
        /// Races the creature has.
        /// </summary>
        public IEnumerable<Race> Races { get; }

        /// <summary>
        /// Determines whether the creature has summoning sickess.
        /// </summary>
        public bool SummoningSickness { get; set; }

        public ICollection<ITriggeredAbility> TriggerAbilities { get; }

        /// <summary>
        /// Creates a battle zone creature.
        /// </summary>
        /// <param name="card"></param>
        public BattleZoneCreature(ICard card) : base(card ?? throw new System.ArgumentNullException(nameof(card)))
        {
        }
    }
}
