using DuelMastersInterfaceModels.Cards;
using System.Collections.ObjectModel;

namespace DuelMastersModels.CardFilters
{
    internal abstract class SpellFilter : CardFilter
    {
        internal abstract ReadOnlyCollection<ISpell> FilteredSpells { get; }
    }
}
