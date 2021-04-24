using DuelMastersModels.Effects.OneShotEffects;

namespace DuelMastersModels.Abilities
{
    /// <summary>
    /// Abstract class for spell, activated and trigger abilities. Non-static abilities are resolvable abilities.
    /// </summary>
    public abstract class NonStaticAbility : Ability
    {
        /// <summary>
        /// Abilities can generate one-shot effects or continuous effects.
        /// </summary>
        public ReadOnlyOneShotEffectCollection Effects { get; private set; }

        /// <summary>
        /// The controller of a triggered ability on the stack (other than a delayed triggered ability) is the player who controlled the ability’s source when it triggered, or, if it had no controller, the player who owned the ability’s source when it triggered.
        /// </summary>
        public Player Controller { get; set; }

        private int _effectIndex = 0;

        /// <summary>
        /// Used in creating non-static abilities.
        /// </summary>
        /// <param name="effects">Effects the ability generates.</param>
        protected NonStaticAbility(ReadOnlyOneShotEffectCollection effects)
        {
            Effects = effects;
        }

        /// <summary>
        /// Applies the next effect generated due to the resolution of the ability. Returns null after all effects of the ability have been applied.
        /// </summary>
        /// <param name="duel"></param>
        /// <returns></returns>
        public PlayerActionWithEndInformation ContinueResolution(Duel duel)
        {
            return _effectIndex < Effects.Count
                ? new PlayerActionWithEndInformation(Effects[_effectIndex++].Apply(duel, Controller), false)
                : new PlayerActionWithEndInformation(null, true);
            //TODO: consider all effects.
            //return PlayerActions[0].Apply(duel, player);
        }

        /*
        internal PlayerActions.PlayerAction ContinueResolution(Duel duel)
        {
            return _effectIndex < Effects.Count
                ? Effects[_effectIndex++].Apply(duel, Controller)
                : null;
        }
        */
    }
}
