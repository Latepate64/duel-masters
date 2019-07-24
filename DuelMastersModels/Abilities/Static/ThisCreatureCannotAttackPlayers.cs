﻿using DuelMastersModels.Cards;

namespace DuelMastersModels.Abilities.Static
{
    public class ThisCreatureCannotAttackPlayers : StaticAbilityForCreature
    {
        public ThisCreatureCannotAttackPlayers(Creature creature) : base(new Effects.ContinuousEffects.CannotAttackPlayersEffect(new Effects.Periods.Indefinite(), new CardFilters.TargetCreatureFilter(creature)), StaticAbilityForCreatureActivityCondition.WhileThisCreatureIsInTheBattleZone)
        { }
    }
}