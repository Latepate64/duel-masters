namespace DuelMastersModels.Effects.ContinuousEffects
{
    internal class BlockerEffect : CreatureContinuousEffect
    {
        internal BlockerEffect(Periods.Period period, CardFilters.CreatureFilter creatureFilter) : base(period, creatureFilter) { }
    }
}
