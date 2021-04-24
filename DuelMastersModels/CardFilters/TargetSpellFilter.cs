using DuelMastersInterfaceModels.Cards;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DuelMastersModels.CardFilters
{
    internal class TargetSpellFilter : SpellFilter
    {
        internal ISpell Spell { get; private set; }

        internal TargetSpellFilter(ISpell spell)
        {
            Spell = spell;
        }

        internal override ReadOnlyCollection<ISpell> FilteredSpells => new ReadOnlyCollection<ISpell>(new List<ISpell> { Spell });
    }
}

