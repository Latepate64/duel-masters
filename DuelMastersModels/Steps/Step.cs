using DuelMastersInterfaceModels.Choices;

namespace DuelMastersModels.Steps
{
    public enum StepState
    {
        NotStarted,
        TurnBasedAction,
        ResolveAbilities,
        PriorityAction,
        Over,
    }

    public abstract class Step
    {
        /// <summary>
        /// The player whose turn it is.
        /// </summary>
        public Player ActivePlayer { get; }

        public StepState State { get; set; } = StepState.NotStarted;

        protected Step(Player activePlayer)
        {
            ActivePlayer = activePlayer;
        }

        public (IChoice, bool) ResolveAbility()
        {
            State = StepState.ResolveAbilities;
            // TODO: Add functionality for resolving abilities.
            return (null, false); //TODO: Consider if any ability was resolved
        }

        public IChoice Start()
        {
            if (this is ITurnBasedActionStep turnBasedActionStep)
            {
                State = StepState.TurnBasedAction;
                IChoice choice = turnBasedActionStep.PerformTurnBasedAction();
                if (choice != null)
                {
                    return choice;
                }
            }
            return Proceed();
        }

        public IChoice Proceed()
        {
            IChoice choice = TryToResolveAbility();
            if (choice != null)
            {
                return choice;
            }
            else if (this is IPriorityStep priorityStep)
            {
                choice = priorityStep.PerformPriorityAction();
                if (choice != null)
                {
                    return choice;
                }
            }
            State = StepState.Over;
            return null;
        }

        public abstract Step GetNextStep();

        private IChoice TryToResolveAbility()
        {
            IChoice choice;
            bool anyAbilityResolved;
            (choice, anyAbilityResolved) = ResolveAbility();
            if (choice != null)
            {
                return choice;
            }
            else if (anyAbilityResolved)
            {
                return TryToResolveAbility();
            }
            else
            {
                return null; // There were no abilities to be resolved.
            }
        }
    }
}
