using DuelMastersInterfaceModels.Cards;

namespace DuelMastersModels.Abilities.TriggeredAbilities
{
    public interface ITriggeredAbility : INonStaticAbility
    {
        TriggerCondition TriggerCondition { get; set; }

        TriggeredAbility CreatePendingTriggeredAbility(IPlayer controller, ICard source);
    }
}