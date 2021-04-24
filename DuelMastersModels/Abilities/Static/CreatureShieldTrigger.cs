﻿using DuelMastersModels.Cards;

namespace DuelMastersModels.Abilities.StaticAbilities
{
    internal class CreatureShieldTrigger : StaticAbilityForCreature
    {
        internal CreatureShieldTrigger(ICreature creature) : base(new Effects.ContinuousEffects.CreatureShieldTriggerEffect(new Effects.Periods.Indefinite(), new CardFilters.TargetCreatureFilter<ICreature>(creature)), EffectActivityConditionForCreature.WhileThisCreatureIsInYourHand)
        { }
    }
}
