using DuelMastersInterfaceModels.Cards;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DuelMastersModels.CardFilters
{
    internal class TargetCreatureFilter : CreatureFilter
    {
        internal ICreature Creature { get; private set; }

        internal TargetCreatureFilter(ICreature creature)
        {
            Creature = creature;
        }

        internal override IEnumerable<ICreature> FilteredCreatures => new ReadOnlyCollection<ICreature>(new List<ICreature> { Creature });
    }
}
