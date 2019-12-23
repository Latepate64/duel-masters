using DuelMastersModels.Cards;

namespace DuelMastersModels.PlayerActions.CardSelections
{
    public class UseShieldTrigger : MandatoryCardSelection
    {
        public UseShieldTrigger(Player player, ReadOnlyCardCollection cards) : base(player, cards) { }

        public override PlayerAction Perform(Duel duel, GameCard card)
        {
            Player.Hand.Remove(card, duel);
            duel.PlayCard(card, Player);
            Player.ShieldTriggersToUse.Remove(card);
            return null;
        }
    }
}
