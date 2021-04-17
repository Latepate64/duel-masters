using DuelMastersInterfaceModels.Choices;

namespace DuelMastersModels.Abilities
{
    public class PlayerActionWithEndInformation
    {
        public PlayerActionWithEndInformation(IChoice playerAction, bool resolutionOver)
        {
            PlayerAction = playerAction;
            ResolutionOver = resolutionOver;
        }

        internal IChoice PlayerAction { get; private set; }
        internal bool ResolutionOver { get; private set; }
    }
}
