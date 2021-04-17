using DuelMastersInterfaceModels.Choices;

namespace DuelMastersModels.Steps
{
    /// <summary>
    /// 502.1. The active player draws a card. This is a turn-based action.
    /// </summary>
    public class DrawStep : TurnBasedActionStep
    {
        public DrawStep(IPlayer player) : base(player)
        {
        }

        public override IStep GetNextStep()
        {
            return new ChargeStep(ActivePlayer);
        }

        /// <summary>
        /// 702.3b Immediately after the draw step begins, the active player draws a card.
        /// </summary>
        /// <returns></returns>
        public override IChoice PerformTurnBasedAction()
        {
            ActivePlayer.DrawCards(1);
            return null;
        }
    }
}
