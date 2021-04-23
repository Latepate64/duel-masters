using DuelMastersInterfaceModels.Cards;
using DuelMastersModels.Abilities.TriggeredAbilities;
using System.Collections.Generic;

namespace DuelMastersModels.Cards
{
    internal class GraveyardCreature : GraveyardCard, ICreature
    {
        public int Power { get; }
        public IEnumerable<Race> Races { get; }
        public bool SummoningSickness { get; set; }
        public ICollection<ITriggeredAbility> TriggerAbilities { get; }

        internal GraveyardCreature(ICard card) : base(card)
        {
        }
    }
}
