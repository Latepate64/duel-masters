using DuelMastersModels.Abilities;
using DuelMastersModels.Abilities.Static;
using DuelMastersModels.Effects.OneShotEffects;
using DuelMastersModels.Factories;
using System.Collections.Generic;
using System.Linq;

namespace DuelMastersModels.Cards
{
    public class GameSpell : GameCard
    {
        public ReadOnlySpellAbilityCollection SpellAbilities => (BaseCard as Spell).SpellAbilities;

        public GameSpell(Spell spell, int gameId, Player owner) : base(spell, gameId)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                IEnumerable<string> textParts = Text.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries).Where(t => !(t.StartsWith("(", System.StringComparison.CurrentCulture) && t.EndsWith(")", System.StringComparison.CurrentCulture)));
                foreach (string textPart in textParts)
                {
                    StaticAbility staticAbility = StaticAbilityFactory.ParseStaticAbilityForSpell(textPart, this);
                    if (staticAbility != null)
                    {
                        Abilities.Add(staticAbility);
                    }
                    else
                    {
                        ReadOnlyOneShotEffectCollection effects = EffectFactory.ParseOneShotEffect(textPart, owner);
                        if (effects != null)
                        {
                            Abilities.Add(new SpellAbility(effects, owner, this));
                        }
                        else
                        {
                            Duel.NotParsedAbilities.Add(textPart);
                        }
                    }
                }
            }
        }
    }
}
