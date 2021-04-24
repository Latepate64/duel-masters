using DuelMastersModels.Cards;
using System.Collections.Generic;

namespace DuelMastersModels.CardFilters
{
    internal abstract class CreatureFilter : CardFilter
    {
        internal abstract IEnumerable<Creature> FilteredCreatures { get; }
    }
}
