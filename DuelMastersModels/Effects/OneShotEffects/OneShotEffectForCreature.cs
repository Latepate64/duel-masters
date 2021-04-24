using DuelMastersModels.Cards;

namespace DuelMastersModels.Effects.OneShotEffects
{
    internal abstract class OneShotEffectForCreature : OneShotEffect
    {
        internal Creature Creature { get; private set; }

        protected OneShotEffectForCreature(Creature creature)
        {
            Creature = creature;
        }
    }
}
