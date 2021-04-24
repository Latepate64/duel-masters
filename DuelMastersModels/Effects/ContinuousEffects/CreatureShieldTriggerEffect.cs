using DuelMastersModels.Cards;

namespace DuelMastersModels.Effects.ContinuousEffects
{
    internal class CreatureShieldTriggerEffect : CreatureContinuousEffect<ICreature>
    {
        internal CreatureShieldTriggerEffect(Periods.Period period, CardFilters.CreatureFilter<ICreature> creatureFilter) : base(period, creatureFilter) { }
    }
}
