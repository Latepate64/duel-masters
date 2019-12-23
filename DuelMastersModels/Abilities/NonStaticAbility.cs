using DuelMastersModels.Effects.OneShotEffects;
using DuelMastersModels.PlayerActions;

namespace DuelMastersModels.Abilities
{
    public abstract class NonStaticAbility : Ability
    {
        public ReadOnlyOneShotEffectCollection Effects { get; private set; }

        /// <summary>
        /// 113.8. The controller of a triggered ability on the stack (other than a delayed triggered ability) is the player who controlled the ability’s source when it triggered, or, if it had no controller, the player who owned the ability’s source when it triggered. To determine the controller of a delayed triggered ability, see rules 603.7d–f.
        /// </summary>
        public Player Controller { get; set; }

        /// <summary>
        /// The source of an ability is the object that generated it. The source of an activated ability on the stack is the object whose ability was activated. The source of a triggered ability (other than a delayed triggered ability) on the stack, or one that has triggered and is waiting to be put on the stack, is the object whose ability triggered.
        /// </summary>
        public Cards.GameCard Source { get; private set; }

        private int _effectIndex = 0;

        protected NonStaticAbility(ReadOnlyOneShotEffectCollection effects, Player controller, Cards.GameCard source)
        {
            Effects = effects;
            Controller = controller;
            Source = source;
        }

        /// <summary>
        /// Applies the next effect generated due to the resolution of the ability. Returns null after all effects of the ability have been applied.
        /// </summary>
        /// <param name="duel"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public PlayerActionWithEndInformation ContinueResolution(Duel duel)
        {
            return _effectIndex < Effects.Count
                ? new PlayerActionWithEndInformation(Effects[_effectIndex++].Apply(duel, Controller), false)
                : new PlayerActionWithEndInformation(null, true);
            //TODO: consider all effects.
            //return PlayerActions[0].Apply(duel, player);
            //return Effects.Apply(duel, player);
        }
    }

    public class PlayerActionWithEndInformation
    {
        public PlayerActionWithEndInformation(PlayerAction playerAction, bool end)
        {
            PlayerAction = playerAction;
            End = end;
        }

        public PlayerAction PlayerAction { get; private set; }
        public bool End { get; private set; }
    }
}
