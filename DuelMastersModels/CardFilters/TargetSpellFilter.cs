using DuelMastersModels.Cards;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DuelMastersModels.CardFilters
{
    internal class TargetSpellFilter : SpellFilter
    {
        internal Spell Spell { get; private set; }

        internal TargetSpellFilter(Spell spell)
        {
            Spell = spell;
        }

        internal override ReadOnlyCollection<Spell> FilteredSpells => new(new List<Spell> { Spell });
    }
}

