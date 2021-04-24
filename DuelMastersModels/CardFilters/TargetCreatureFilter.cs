using DuelMastersModels.Cards;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DuelMastersModels.CardFilters
{
    internal class TargetCreatureFilter : CreatureFilter
    {
        internal Creature Creature { get; private set; }

        internal TargetCreatureFilter(Creature creature)
        {
            Creature = creature;
        }

        internal override IEnumerable<Creature> FilteredCreatures => new ReadOnlyCollection<Creature>(new List<Creature> { Creature });
    }
}
