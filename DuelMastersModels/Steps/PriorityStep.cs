using DuelMastersInterfaceModels.Choices;

namespace DuelMastersModels.Steps
{
    public abstract class PriorityStep : Step, IPriorityStep
    {
        protected PriorityStep(Player activePlayer) : base(activePlayer)
        {
        }

        public abstract IChoice PerformPriorityAction();
    }
}
