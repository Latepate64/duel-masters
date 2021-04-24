using DuelMastersInterfaceModels.Cards;

namespace DuelMastersModels.Abilities.StaticAbilities
{
    internal class SpeedAttacker : StaticAbilityForCreature
    {
        internal SpeedAttacker(ICreature creature) : base(new Effects.ContinuousEffects.SpeedAttackerEffect(new Effects.Periods.Indefinite(), new CardFilters.TargetCreatureFilter(creature)), EffectActivityConditionForCreature.WhileThisCreatureIsInTheBattleZone)
        { }
    }
}

