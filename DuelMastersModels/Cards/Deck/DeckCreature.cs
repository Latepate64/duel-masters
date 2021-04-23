using DuelMastersInterfaceModels.Cards;
using DuelMastersModels.Abilities.TriggeredAbilities;
using System.Collections.Generic;

namespace DuelMastersModels.Cards
{
    internal class DeckCreature : DeckCard, ICreature
    {
        public int Power { get; }
        public IEnumerable<Race> Races { get; }
        public bool SummoningSickness { get; set; }
        public ICollection<ITriggeredAbility> TriggerAbilities { get; }

        internal DeckCreature(ICreature creature) : base(creature)
        {
            Power = creature.Power;
            Races = creature.Races;
        }
    }
}
