using DuelMastersModels.Cards;

namespace DuelMastersModels.PlayerActions.CardSelections
{
    public class DeclareShieldTriggers : MultipleCardSelection
    {
        public DeclareShieldTriggers(Player player, ReadOnlyCardCollection cards) : base(player, cards)
        { }

        public override PlayerAction Perform(Duel duel, ReadOnlyCardCollection cards)
        {
            foreach (GameCard card in cards)
            {
                Player.ShieldTriggersToUse.Add(card);
            }
            return null;
        }
    }
}
