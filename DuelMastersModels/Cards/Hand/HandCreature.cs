using DuelMastersInterfaceModels.Cards;
using DuelMastersModels.Abilities.TriggeredAbilities;
using System.Collections.Generic;

namespace DuelMastersModels.Cards
{
    internal class HandCreature : HandCard, IHandCreature
    {
        public int Power { get; }
        public IEnumerable<Race> Races { get; }
        public bool SummoningSickness { get; set; }
        public ICollection<ITriggeredAbility> TriggerAbilities { get; }

        internal HandCreature(ICreature creature) : base(creature)
        {
            Races = creature.Races;
            Power = creature.Power;
        }
    }
}
