using DuelMastersModels.Effects.OneShotEffects;

namespace DuelMastersModels.Abilities.Trigger
{
    /// <summary>
    /// 603.1. Triggered abilities have a trigger condition and an effect. They are written as “[When/Whenever/At] [trigger condition or event], [effect]. [Instructions (if any).]”
    /// </summary>
    public class TriggerAbility : NonStaticAbility
    {
        public TriggerCondition TriggerCondition { get; private set; }

        public TriggerAbility(TriggerCondition triggerCondition, ReadOnlyOneShotEffectCollection effects, Player controller, Cards.GameCard card) : base(effects, controller, card)
        {
            TriggerCondition = triggerCondition;
        }

        /// <summary>
        /// 603.3. Once an ability has triggered, its controller puts it on the stack as an object that’s not a card the next time a player would receive priority.
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public TriggerAbility CreatePendingTriggerAbility(Player controller)
        {
            return new TriggerAbility(TriggerCondition, Effects, controller, Source);
            /*
            var effects = new List<Effect>();
            foreach (var effect in Effects)
            {
                effects.Add(effect.DeepCopy());
            }
            return new TriggerAbility(TriggerCondition, effects);*/
        }
    }
}
