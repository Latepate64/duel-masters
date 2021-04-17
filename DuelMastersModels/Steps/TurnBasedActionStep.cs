using DuelMastersInterfaceModels.Choices;

namespace DuelMastersModels.Steps
{
    public abstract class TurnBasedActionStep : Step, ITurnBasedActionStep
    {
        protected TurnBasedActionStep(IPlayer activePlayer) : base(activePlayer)
        {
        }

        public abstract IChoice PerformTurnBasedAction();
    }
}