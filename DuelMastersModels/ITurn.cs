using DuelMastersInterfaceModels.Choices;
using DuelMastersModels.Managers;
using DuelMastersModels.Steps;
using DuelMastersModels.Zones;

namespace DuelMastersModels
{
    public interface ITurn
    {
        /// <summary>
        /// The player whose turn it is.
        /// </summary>
        IPlayer ActivePlayer { get; }

        /// <summary>
        /// The opponent of the active player.
        /// </summary>
        IPlayer NonActivePlayer { get; }

        /// <summary>
        /// The step that is currently being processed.
        /// </summary>
        IStep CurrentStep { get; }

        IChoice ChangeAndStartStep();
        IChoice Start(IBattleZone battleZone, EventManager eventManager);
    }
}