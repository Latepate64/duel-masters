using DuelMastersInterfaceModels.Choices;

namespace DuelMastersModels.Steps
{
    public interface ITurnBasedActionStep
    {
        IChoice PerformTurnBasedAction();
    }
}