using DuelMastersModels.CardFilters;
using DuelMastersModels.Cards;
using DuelMastersModels.Effects.Periods;

namespace DuelMastersModels.Effects.ContinuousEffects
{
    internal class SpeedAttackerEffect : CreatureContinuousEffect<ICreature>
    {
        internal SpeedAttackerEffect(Period period, CreatureFilter<ICreature> creatureFilter) : base(period, creatureFilter) { }
    }
}