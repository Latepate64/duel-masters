using DuelMastersModels.Cards;

namespace DuelMastersModels.Effects.OneShotEffects
{
    public abstract class OneShotEffectForCreature : OneShotEffect
    {
        public GameCreature Creature { get; private set; }

        protected OneShotEffectForCreature(GameCreature creature)
        {
            Creature = creature;
        }
    }
}
