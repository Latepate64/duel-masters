using DuelMastersModels.CardFilters;
using DuelMastersModels.Cards;
using DuelMastersModels.Effects.Periods;

namespace DuelMastersModels.Effects.ContinuousEffects
{
    internal class AttacksIfAbleEffect : CreatureContinuousEffect<ICreature>
    {
        internal AttacksIfAbleEffect(Period period, CreatureFilter<ICreature> creatureFilter) : base(period, creatureFilter) { }
    }
}