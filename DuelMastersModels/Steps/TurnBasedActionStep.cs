using DuelMastersInterfaceModels.Choices;

namespace DuelMastersModels.Steps
{
    public abstract class TurnBasedActionStep : Step, ITurnBasedActionStep
    {
        protected TurnBasedActionStep(Player activePlayer) : base(activePlayer)
        {
        }

        public abstract IChoice PerformTurnBasedAction();
    }
}