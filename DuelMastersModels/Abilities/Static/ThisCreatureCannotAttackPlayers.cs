using DuelMastersModels.Cards;

namespace DuelMastersModels.Abilities.Static
{
    public class ThisCreatureCannotAttackPlayers : StaticAbilityForCreature
    {
        public ThisCreatureCannotAttackPlayers(GameCreature creature) : base(new Effects.ContinuousEffects.CannotAttackPlayersEffect(new Effects.Periods.Indefinite(), new CardFilters.TargetCreatureFilter(creature)), StaticAbilityForCreatureActivityCondition.WhileThisCreatureIsInTheBattleZone)
        { }
    }
}
