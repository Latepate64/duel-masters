using DuelMastersModels.CardFilters;
using DuelMastersModels.Effects.Periods;

namespace DuelMastersModels.Effects.ContinuousEffects
{
    internal class AttacksIfAbleEffect : CreatureContinuousEffect
    {
        internal AttacksIfAbleEffect(Period period, CreatureFilter creatureFilter) : base(period, creatureFilter) { }
    }
}