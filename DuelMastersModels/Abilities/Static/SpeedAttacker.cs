﻿using DuelMastersModels.Cards;

namespace DuelMastersModels.Abilities.Static
{
    internal class SpeedAttacker : StaticAbilityForCreature
    {
        internal SpeedAttacker(Creature creature) : base(new Effects.ContinuousEffects.SpeedAttackerEffect(new Effects.Periods.Indefinite(), new CardFilters.TargetCreatureFilter(creature)), EffectActivityConditionForCreature.WhileThisCreatureIsInTheBattleZone)
        { }
    }
}
