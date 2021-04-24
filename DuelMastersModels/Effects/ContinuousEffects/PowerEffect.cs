namespace DuelMastersModels.Effects.ContinuousEffects
{
    internal class PowerEffect : CreatureContinuousEffect
    {
        internal int Power { get; private set; }

        internal PowerEffect(Periods.Period period, CardFilters.CreatureFilter creatureFilter, int power) : base(period, creatureFilter) { Power = power; }
    }
}
