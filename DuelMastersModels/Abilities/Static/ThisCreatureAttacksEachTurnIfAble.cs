using DuelMastersInterfaceModels.Cards;

namespace DuelMastersModels.Abilities.StaticAbilities
{
    internal class ThisCreatureAttacksEachTurnIfAble : StaticAbilityForCreature
    {
        internal ThisCreatureAttacksEachTurnIfAble(ICreature creature) : base(new Effects.ContinuousEffects.AttacksIfAbleEffect(new Effects.Periods.Indefinite(), new CardFilters.TargetCreatureFilter(creature)), EffectActivityConditionForCreature.WhileThisCreatureIsInTheBattleZone)
        { }
    }
}
