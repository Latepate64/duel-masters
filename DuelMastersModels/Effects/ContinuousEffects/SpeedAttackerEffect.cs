using DuelMastersModels.CardFilters;
using DuelMastersModels.Effects.Periods;

namespace DuelMastersModels.Effects.ContinuousEffects
{
    internal class SpeedAttackerEffect : CreatureContinuousEffect
    {
        internal SpeedAttackerEffect(Period period, CreatureFilter creatureFilter) : base(period, creatureFilter) { }
    }
}