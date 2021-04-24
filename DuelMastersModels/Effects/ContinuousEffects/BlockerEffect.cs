using DuelMastersModels.Cards;

namespace DuelMastersModels.Effects.ContinuousEffects
{
    internal class BlockerEffect : CreatureContinuousEffect<ICreature>
    {
        internal BlockerEffect(Periods.Period period, CardFilters.CreatureFilter<ICreature> creatureFilter) : base(period, creatureFilter) { }
    }
}
