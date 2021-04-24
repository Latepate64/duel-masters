namespace DuelMastersModels.Effects.ContinuousEffects
{
    internal class CannotAttackPlayersEffect : CreatureContinuousEffect
    {
        internal CannotAttackPlayersEffect(Periods.Period period, CardFilters.CreatureFilter creatureFilter) : base(period, creatureFilter) { }
    }
}
