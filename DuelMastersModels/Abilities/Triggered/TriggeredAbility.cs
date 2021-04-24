using DuelMastersModels.Cards;
using DuelMastersModels.Effects.OneShotEffects;

namespace DuelMastersModels.Abilities.TriggeredAbilities
{
    /// <summary>
    /// 603.1. Triggered abilities have a trigger condition and an effect. They are written as “[When/Whenever/At] [trigger condition or event], [effect]. [Instructions (if any).]”
    /// </summary>
    public class TriggeredAbility : NonStaticAbility
    {
        public TriggerCondition TriggerCondition { get; set; }

        public TriggeredAbility(TriggerCondition triggerCondition, ReadOnlyOneShotEffectCollection effects) : base(effects)
        {
            TriggerCondition = triggerCondition;
        }

        public TriggeredAbility(TriggerCondition triggerCondition, OneShotEffect effect) : this(triggerCondition, new ReadOnlyOneShotEffectCollection(effect)) { }

        /// <summary>
        /// 603.3. Once an ability has triggered, its controller puts it on the stack as an object that’s not a card the next time a player would receive priority.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public TriggeredAbility CreatePendingTriggeredAbility(Player controller, Card source)
        {
            return new TriggeredAbility(TriggerCondition, Effects) { Controller = controller, Source = source };
            /*
            var effects = new List<Effect>();
            foreach (var effect in Effects)
            {
                effects.Add(effect.DeepCopy());
            }
            return new TriggeredAbility(TriggerCondition, effects);*/
        }
    }
}
