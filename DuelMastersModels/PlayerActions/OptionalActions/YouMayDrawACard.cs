﻿namespace DuelMastersModels.PlayerActions.OptionalActions
{
    /// <summary>
    /// Player may draw a card.
    /// </summary>
    public class YouMayDrawACard : OptionalAction
    {
        internal YouMayDrawACard(Player player) : base(player) { }

        internal override PlayerAction Perform(Duel duel, bool takeAction)
        {
            if (takeAction)
            {
                return new AutomaticActions.DrawCard(Player);
            }
            else
            {
                return null;
            }
        }
    }
}
