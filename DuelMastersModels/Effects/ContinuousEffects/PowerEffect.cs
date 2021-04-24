using DuelMastersModels.Cards;

namespace DuelMastersModels.Effects.ContinuousEffects
{
    internal class PowerEffect : CreatureContinuousEffect<ICreature>
    {
        internal int Power { get; private set; }

        internal PowerEffect(Periods.Period period, CardFilters.CreatureFilter<ICreature> creatureFilter, int power) : base(period, creatureFilter) { Power = power; }
    }
}
