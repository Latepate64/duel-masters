using DuelMastersModels.Cards;

namespace DuelMastersModels.CardFilters
{
    public class TargetSpellFilter : SpellFilter
    {
        public GameSpell Spell { get; private set; }

        public TargetSpellFilter(GameSpell spell)
        {
            Spell = spell;
        }

        public override ReadOnlySpellCollection FilteredSpells => new ReadOnlySpellCollection(Spell);
    }
}

