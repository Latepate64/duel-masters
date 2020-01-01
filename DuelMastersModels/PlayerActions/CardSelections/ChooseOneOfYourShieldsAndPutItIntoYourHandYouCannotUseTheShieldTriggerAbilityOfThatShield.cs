﻿using System;
using DuelMastersModels.Cards;

namespace DuelMastersModels.PlayerActions.CardSelections
{
    /// <summary>
    /// Player chooses one of their shields and puts it into their hand. They can't use the "shield trigger" ability of that shield.
    /// </summary>
    public class ChooseOneOfYourShieldsAndPutItIntoYourHandYouCannotUseTheShieldTriggerAbilityOfThatShield : MandatoryCardSelection
    {
        internal ChooseOneOfYourShieldsAndPutItIntoYourHandYouCannotUseTheShieldTriggerAbilityOfThatShield(Player player) : base(player, new ReadOnlyCardCollection(player.ShieldZone.Cards)) { }

        internal override PlayerAction Perform(Duel duel, Card card)
        {
            if (duel == null)
            {
                throw new ArgumentNullException("duel");
            }
            else if (card != null)
            {
                duel.PutFromShieldZoneToHand(Player, card, false);
            }
            return null;
        }
    }
}
