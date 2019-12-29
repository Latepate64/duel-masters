﻿using DuelMastersModels.GameActions.TurnBasedActions;
using DuelMastersModels.PlayerActions;

namespace DuelMastersModels.Steps
{
    /// <summary>
    /// 501.1 The active player determines which cards they control will untap. Then they untap them all simultaneously. This is a turn-based action. Normally, all of a player’s cards untap, but effects can keep one or more of a player’s cards from untapping.
    /// </summary>
    internal class StartOfTurnStep : Step
    {
        internal StartOfTurnStep(Player player) : base(player)
        {
        }

        internal override PlayerAction PlayerActionRequired(Duel duel)
        {
            return null;
        }

        /// <summary>
        /// 702.3a The active player untaps their cards in the battle zone and mana zone simultaneously. 
        /// </summary>
        internal override PlayerAction ProcessTurnBasedActions(Duel duel)
        {
            new UntapCards().Perform(duel);
            return null;
        }
    }
}
