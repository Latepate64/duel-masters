﻿using DuelMastersModels.PlayerActions;

namespace DuelMastersModels.Effects.OneShotEffects
{
    public class ThisCreatureGetsXPowerUntilTheEndOfTheTurn : OneShotEffectForCreature
    {
        public int Power { get; private set; }

        public ThisCreatureGetsXPowerUntilTheEndOfTheTurn(Cards.Creature creature, int power) : base(creature)
        {
            Power = power;
        }

        public override PlayerAction Apply(Duel duel, Player player)
        {
            duel.ContinuousEffects.Add(new ContinuousEffects.PowerEffect(new Periods.UntilTheEndOfTheTurn(), new CardFilters.TargetCreatureFilter(Creature), Power));
            return null;
        }
    }
}