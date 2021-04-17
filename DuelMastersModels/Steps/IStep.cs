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

    public interface IStep
    {
        IPlayer ActivePlayer { get; }
        StepState State { get; set; }

        IStep GetNextStep();
        IChoice Proceed();
        (IChoice, bool) ResolveAbility();
        IChoice Start();
    }
}