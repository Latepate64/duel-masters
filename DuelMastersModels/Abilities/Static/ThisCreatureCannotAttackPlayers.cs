using DuelMastersModels.Cards;

namespace DuelMastersModels.Abilities.StaticAbilities
{
    internal class ThisCreatureCannotAttackPlayers : StaticAbilityForCreature
    {
        internal ThisCreatureCannotAttackPlayers(Creature creature) : base(new Effects.ContinuousEffects.CannotAttackPlayersEffect(new Effects.Periods.Indefinite(), new CardFilters.TargetCreatureFilter(creature)), EffectActivityConditionForCreature.WhileThisCreatureIsInTheBattleZone)
        { }
    }
}
