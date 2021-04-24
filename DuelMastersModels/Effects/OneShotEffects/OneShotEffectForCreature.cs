using DuelMastersInterfaceModels.Cards;

namespace DuelMastersModels.Effects.OneShotEffects
{
    internal abstract class OneShotEffectForCreature : OneShotEffect
    {
        internal ICreature Creature { get; private set; }

        protected OneShotEffectForCreature(ICreature creature)
        {
            Creature = creature;
        }
    }
}
