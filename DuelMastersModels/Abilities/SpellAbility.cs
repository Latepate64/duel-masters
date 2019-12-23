using DuelMastersModels.Effects.OneShotEffects;

namespace DuelMastersModels.Abilities
{
    public class SpellAbility : NonStaticAbility
    {
        public SpellAbility(ReadOnlyOneShotEffectCollection effects, Player controller, Cards.GameSpell spell) : base(effects, controller, spell)
        { }
    }
}
