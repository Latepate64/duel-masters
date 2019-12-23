using DuelMastersModels.Abilities;
using System.Collections.ObjectModel;
using System.Linq;

namespace DuelMastersModels.Cards
{
    public class Spell : Card
    {
        public ReadOnlySpellAbilityCollection SpellAbilities => new ReadOnlySpellAbilityCollection(Abilities.Where(a => a is SpellAbility).Cast<SpellAbility>());

        public Spell() : base()
        {
        }

        /// <summary>
        /// Creates a spell.
        /// </summary>
        public Spell(string name, string set, string id, Collection<string> civilizations, string rarity, int cost, string text, string flavor, string illustrator) : base(name, set, id, civilizations, rarity, cost, text, flavor, illustrator)
        {
        }
    }
}
