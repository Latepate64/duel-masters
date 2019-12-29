﻿using DuelMastersModels.Cards;
using DuelMastersModels.Steps;
using System;

namespace DuelMastersModels.PlayerActions.CardSelections
{
    public class PayCost : MandatoryMultipleCardSelection
    {
        internal int Cost { get; set; }

        internal PayCost(Player player, ReadOnlyCardCollection cards, int cost) : base(player, cost, cards)
        {
            Cost = cost;
        }

        internal static bool Validate(ReadOnlyCardCollection cards, Card cardToBeUsed)
        {
            return Duel.CanBeUsed(cardToBeUsed, cards);
        }

        internal override PlayerAction Perform(Duel duel, ReadOnlyCardCollection cards)
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
                foreach (Card card in cards)
                {
                    card.Tapped = true;
                }
                duel.UseCard(mainStep.CardToBeUsed);
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
