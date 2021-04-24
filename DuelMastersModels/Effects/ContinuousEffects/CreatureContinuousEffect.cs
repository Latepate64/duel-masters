using DuelMastersModels.CardFilters;
using DuelMastersModels.Effects.Periods;

namespace DuelMastersModels.Effects.ContinuousEffects
{
    internal abstract class CreatureContinuousEffect : ContinuousEffect
    {
        internal CreatureFilter CreatureFilter { get; private set; }

        protected CreatureContinuousEffect(Period period, CreatureFilter creatureFilter) : base(period)
        {
            CreatureFilter = creatureFilter;
        }
    }
}
