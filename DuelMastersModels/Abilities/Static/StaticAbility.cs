using DuelMastersModels.Effects.ContinuousEffects;

namespace DuelMastersModels.Abilities.StaticAbilities
{
    /// <summary>
    /// Static abilities do something all the time rather than being activated or triggered. They are written as statements, and they’re simply true.
    /// </summary>
    public abstract class StaticAbility : Ability
    {
        /// <summary>
        /// Static abilities create continuous effects, some of which are prevention effects or replacement effects. These effects are active as long as the card with the ability remains on the battle zone and has the ability, or as long as the card with the ability remains in the appropriate zone.
        /// </summary>
        public ReadOnlyContinuousEffectCollection ContinuousEffects { get; private set; }

        /// <summary>
        /// Creates a static ability.
        /// </summary>
        /// <param name="continuousEffect">Continuous effect created by the ability.</param>
        /// 
        protected StaticAbility(ContinuousEffect continuousEffect)
        {
            ContinuousEffects = new ReadOnlyContinuousEffectCollection(continuousEffect);
        }
    }
}
