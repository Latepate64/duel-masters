using DuelMastersModels.Cards;

namespace DuelMastersModels.Effects.ContinuousEffects
{
    internal class CannotAttackPlayersEffect : CreatureContinuousEffect<ICreature>
    {
        internal CannotAttackPlayersEffect(Periods.Period period, CardFilters.CreatureFilter<ICreature> creatureFilter) : base(period, creatureFilter) { }
    }
}
