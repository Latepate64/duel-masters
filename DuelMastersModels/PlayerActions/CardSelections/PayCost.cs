using DuelMastersModels.Cards;
using DuelMastersModels.Steps;
using System;

namespace DuelMastersModels.PlayerActions.CardSelections
{
    public class PayCost : MandatoryMultipleCardSelection
    {
        public int Cost { get; set; }

        public PayCost(Player player, ReadOnlyCardCollection cards, int cost) : base(player, cost, cards)
        {
            Cost = cost;
        }

        public static bool Validate(ReadOnlyCardCollection cards, GameCard cardToBeUsed)
        {
            return Duel.CanBeUsed(cardToBeUsed, cards);
        }

        public override PlayerAction Perform(Duel duel, ReadOnlyCardCollection cards)
        {
            if (duel == null)
            {
                throw new ArgumentNullException("duel");
            }
            if (cards == null)
            {
                throw new ArgumentNullException("cards");
            }
            else if (duel.CurrentTurn.CurrentStep is MainStep mainStep)
            {
                foreach (GameCard card in cards)
                {
                    card.Tapped = true;
                }
                duel.PlayCard(mainStep.CardToBeUsed, Player);
                mainStep.CardToBeUsed = null;
                mainStep.State = MainStepState.Use;
                return null;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
