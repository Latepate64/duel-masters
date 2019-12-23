using DuelMastersModels.Cards;

namespace DuelMastersModels.Abilities.Static
{
    public class Blocker : StaticAbilityForCreature
    {
        public Blocker(GameCreature creature) : base(new Effects.ContinuousEffects.BlockerEffect(new Effects.Periods.Indefinite(), new CardFilters.TargetCreatureFilter(creature)), StaticAbilityForCreatureActivityCondition.WhileThisCreatureIsInTheBattleZone)
        { }
    }
}
