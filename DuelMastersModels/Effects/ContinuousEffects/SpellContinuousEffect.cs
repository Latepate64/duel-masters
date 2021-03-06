﻿using DuelMastersModels.CardFilters;
using DuelMastersModels.Effects.Periods;

namespace DuelMastersModels.Effects.ContinuousEffects
{
    internal abstract class SpellContinuousEffect : ContinuousEffect
    {
        internal SpellFilter SpellFilter { get; private set; }

        protected SpellContinuousEffect(Period period, SpellFilter spellFilter) : base(period)
        {
            SpellFilter = spellFilter;
        }
    }
}
