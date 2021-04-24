using DuelMastersInterfaceModels.Cards;
using System.Collections.Generic;

namespace DuelMastersModels.CardFilters
{
    internal abstract class CreatureFilter : CardFilter
    {
        internal abstract IEnumerable<ICreature> FilteredCreatures { get; }
    }
}
