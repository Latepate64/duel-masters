using DuelMastersModels.Cards;
using DuelMastersModels.Steps;
using System;

namespace DuelMastersModels.PlayerActions.CardSelections
{
    public class UseCard : OptionalCardSelection
    {
        public UseCard(Player player, ReadOnlyCardCollection cards) : base(player, cards)
        { }

        public override PlayerAction Perform(Duel duel, GameCard card)
        {
            if (duel == null)
            {
                throw new ArgumentNullException("duel");
            }
            else if (duel.CurrentTurn.CurrentStep is MainStep mainStep)
            {
                if (card != null)
                {
                    // 601.1a The card leaves the zone it is currently in (usually the player's hand) and is moved to the anywhere zone.
                    Player.Hand.Remove(card, duel);
                    mainStep.CardToBeUsed = card;
                    mainStep.State = MainStepState.Pay;
                }
                else
                {
                    mainStep.State = MainStepState.MustBeEnded;
                }
                return null;
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}