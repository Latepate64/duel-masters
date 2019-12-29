﻿using DuelMastersModels.Effects.Periods;

namespace DuelMastersModels.Effects.ContinuousEffects
{
    /// <summary>
    /// A continuous effect modifies characteristics of objects, modifies control of objects, or affects players or the rules of the game, for a fixed or indefinite period. A continuous effect may be generated by the resolution of a spell or ability.
    /// </summary>
    public abstract class ContinuousEffect : Effect
    {
        /// <summary>
        /// A continuous effect generated by the resolution of a spell or ability lasts as long as stated by the spell or ability creating it (such as “until end of turn”). If no duration is stated, it lasts until the end of the game.
        /// </summary>
        public Period Period { get; private set; }

        /// <summary>
        /// Creates a continuous effect.
        /// </summary>
        /// <param name="period"></param>
        protected ContinuousEffect(Period period) : base()
        {
            Period = period;
        }
    }
}
