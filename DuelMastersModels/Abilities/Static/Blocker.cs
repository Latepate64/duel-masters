using DuelMastersInterfaceModels.Cards;

namespace DuelMastersModels.Abilities.StaticAbilities
{
    internal class Blocker : StaticAbilityForCreature
    {
        internal Blocker(ICreature creature) : base(new Effects.ContinuousEffects.BlockerEffect(new Effects.Periods.Indefinite(), new CardFilters.TargetCreatureFilter(creature)), EffectActivityConditionForCreature.WhileThisCreatureIsInTheBattleZone)
        { }
    }
}
